using MediCareClinic.Data;
using MediCareClinic.Models;
using MediCareClinic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace MediCareClinic.Controllers;

[Authorize(Roles = "Patient,Admin")]
public class AppointmentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AppointmentsController(
        AppDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int doctorId, DateTime? date)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Specialty)
            .Include(d => d.WorkingDays)
            .ThenInclude(x => x.WorkingDay)
            .FirstOrDefaultAsync(d => d.Id == doctorId && d.IsActive);

        if (doctor == null)
            return NotFound();

        var availableDates = BuildAvailableDates(doctor);

        DateTime selectedDate;

        if (date.HasValue &&
            availableDates.Any(x => x.Date == date.Value.Date))
        {
            selectedDate = date.Value.Date;
        }
        else
        {
            selectedDate = availableDates.FirstOrDefault()?.Date
                           ?? DateTime.Today.AddDays(1);
        }

        return View(await BuildBookingVm(
            doctor,
            selectedDate
        ));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Specialty)
            .Include(d => d.WorkingDays)
            .ThenInclude(x => x.WorkingDay)
            .FirstOrDefaultAsync(d =>
                d.Id == model.DoctorId &&
                d.IsActive);

        if (doctor == null)
            return NotFound();

        model.Doctor = doctor;

        model.AvailableDates = BuildAvailableDates(doctor);

        model.Slots = await GetSlots(
            doctor,
            model.AppointmentDate
        );

        if (model.AppointmentDate.Date < DateTime.Today)
        {
            ModelState.AddModelError(
                nameof(model.AppointmentDate),
                "Please choose today or a future date."
            );
        }

        var selectedDateIsAvailable = model.AvailableDates
            .Any(x => x.Date == model.AppointmentDate.Date);

        if (!selectedDateIsAvailable)
        {
            ModelState.AddModelError(
                nameof(model.AppointmentDate),
                "Please choose one of the available days."
            );
        }

        if (!model.Slots.Any(x =>
                x.Time == model.AppointmentTime &&
                !x.IsBooked))
        {
            ModelState.AddModelError(
                nameof(model.AppointmentTime),
                "Please choose a free time slot."
            );
        }

        if (!ModelState.IsValid)
            return View(model);

        var patientId = _userManager.GetUserId(User);

        if (patientId == null)
            return Challenge();

        var duplicate = await _context.Appointments.AnyAsync(a =>
            a.DoctorId == model.DoctorId &&
            a.AppointmentDate == model.AppointmentDate.Date &&
            a.AppointmentTime == model.AppointmentTime &&
            a.Status != "Cancelled");

        if (duplicate)
        {
            ModelState.AddModelError(
                nameof(model.AppointmentTime),
                "This slot was just booked. Please choose another time."
            );

            model.Slots = await GetSlots(
                doctor,
                model.AppointmentDate
            );

            return View(model);
        }

        var appointment = new Appointment
        {
            DoctorId = model.DoctorId,
            PatientId = patientId,
            AppointmentDate = model.AppointmentDate.Date,
            AppointmentTime = model.AppointmentTime,
            Notes = model.Notes,
            Status = "Pending",
            BookingNumber =
                $"MC-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(100, 999)}"
        };

        _context.Appointments.Add(appointment);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                nameof(model.AppointmentTime),
                "This slot is no longer available. Please choose another time."
            );

            model.Slots = await GetSlots(
                doctor,
                model.AppointmentDate
            );

            return View(model);
        }

        TempData["Success"] =
            $"Appointment booked successfully. Booking number: {appointment.BookingNumber}";

        return RedirectToAction(nameof(MyAppointments));
    }

    [HttpGet]
    public async Task<IActionResult> Slots(int doctorId, DateTime date)
    {
        var doctor = await _context.Doctors
            .Include(d => d.WorkingDays)
            .ThenInclude(x => x.WorkingDay)
            .FirstOrDefaultAsync(d =>
                d.Id == doctorId &&
                d.IsActive);

        if (doctor == null)
            return NotFound();

        return Json(await GetSlots(
            doctor,
            date.Date
        ));
    }

    [HttpGet]
    public async Task<IActionResult> MyAppointments()
    {
        var id = _userManager.GetUserId(User);

        var items = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialty)
            .Where(a => a.PatientId == id)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.AppointmentTime)
            .ToListAsync();

        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = _userManager.GetUserId(User);

        var appt = await _context.Appointments
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.PatientId == userId);

        if (appt == null)
            return NotFound();

        if (!CanModify(appt))
        {
            TempData["Error"] =
                "This appointment can no longer be changed.";

            return RedirectToAction(nameof(MyAppointments));
        }

        appt.Status = "Cancelled";

        await _context.SaveChangesAsync();

        TempData["Success"] =
            "Appointment cancelled successfully.";

        return RedirectToAction(nameof(MyAppointments));
    }

    [HttpGet]
    public async Task<IActionResult> Reschedule(int id)
    {
        var userId = _userManager.GetUserId(User);

        var appt = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialty)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.WorkingDays)
            .ThenInclude(x => x.WorkingDay)
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.PatientId == userId);

        if (appt == null)
            return NotFound();

        if (!CanModify(appt))
        {
            TempData["Error"] =
                "This appointment can no longer be rescheduled.";

            return RedirectToAction(nameof(MyAppointments));
        }

        return View(await BuildBookingVm(
            appt.Doctor!,
            appt.AppointmentDate,
            appt.Notes
        ));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reschedule(
        int id,
        BookingViewModel model)
    {
        var userId = _userManager.GetUserId(User);

        var appt = await _context.Appointments
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialty)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.WorkingDays)
            .ThenInclude(x => x.WorkingDay)
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.PatientId == userId);

        if (appt == null)
            return NotFound();

        if (!CanModify(appt))
        {
            TempData["Error"] =
                "This appointment can no longer be rescheduled.";

            return RedirectToAction(nameof(MyAppointments));
        }

        model.DoctorId = appt.DoctorId;
        model.Doctor = appt.Doctor!;

        model.AvailableDates =
            BuildAvailableDates(appt.Doctor!);

        model.Slots =
            await GetSlots(
                appt.Doctor!,
                model.AppointmentDate
            );

        var sameOriginal =
            model.AppointmentDate.Date == appt.AppointmentDate &&
            model.AppointmentTime == appt.AppointmentTime;

        if (!sameOriginal &&
            !model.Slots.Any(x =>
                x.Time == model.AppointmentTime &&
                !x.IsBooked))
        {
            ModelState.AddModelError(
                nameof(model.AppointmentTime),
                "Please choose a free time slot."
            );
        }

        if (!ModelState.IsValid)
            return View(model);

        appt.AppointmentDate =
            model.AppointmentDate.Date;

        appt.AppointmentTime =
            model.AppointmentTime;

        appt.Notes =
            model.Notes;

        await _context.SaveChangesAsync();

        TempData["Success"] =
            $"Appointment rescheduled successfully. Booking number: {appt.BookingNumber}";

        return RedirectToAction(nameof(MyAppointments));
    }

    private bool CanModify(Appointment a)
    {
        return a.Status != "Cancelled" &&
               (a.AppointmentDate.Date - DateTime.Today).TotalDays >= 1;
    }

    private async Task<BookingViewModel> BuildBookingVm(
        Doctor doctor,
        DateTime date,
        string? notes = null)
    {
        var availableDates =
            BuildAvailableDates(doctor);

        var selectedDate =
            availableDates.Any(x => x.Date == date.Date)
                ? date.Date
                : availableDates.FirstOrDefault()?.Date
                  ?? DateTime.Today.AddDays(1);

        return new BookingViewModel
        {
            DoctorId = doctor.Id,
            Doctor = doctor,
            AppointmentDate = selectedDate,
            AppointmentTime = "",
            Notes = notes,
            AvailableDates = availableDates,
            Slots = await GetSlots(
                doctor,
                selectedDate
            )
        };
    }

    private List<AvailableDateViewModel> BuildAvailableDates(
        Doctor doctor)
    {
        var dates = new List<AvailableDateViewModel>();

        var workingDays = doctor.WorkingDays
            .Where(x => x.WorkingDay != null)
            .Select(x => x.WorkingDay.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        for (int i = 1; i < 30; i++)
        {
            var date = DateTime.Today.AddDays(i);

            var dayName = date.DayOfWeek.ToString();

            if (!workingDays.Contains(dayName))
                continue;

            dates.Add(new AvailableDateViewModel
            {
                Date = date,
                DisplayName =
                    date.ToString(
                        "dddd, dd MMMM yyyy",
                        CultureInfo.InvariantCulture)
            });
        }

        return dates;
    }

    private async Task<List<TimeSlotViewModel>> GetSlots(
        Doctor doctor,
        DateTime date)
    {
        var day =
            date.DayOfWeek.ToString();

        var schedule =
            doctor.WorkingDays.FirstOrDefault(x =>
                x.WorkingDay.Name.Equals(
                    day,
                    StringComparison.OrdinalIgnoreCase));

        if (schedule == null)
            return new List<TimeSlotViewModel>();

        var booked =
            await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctor.Id &&
                    a.AppointmentDate == date &&
                    a.Status != "Cancelled")
                .Select(a => a.AppointmentTime)
                .ToListAsync();

        var slots =
            new List<TimeSlotViewModel>();

        var duration =
            TimeSpan.FromMinutes(
                Math.Max(
                    15,
                    doctor.VisitDurationMinutes));

        for (
            var t = schedule.StartTime;
            t + duration <= schedule.EndTime;
            t += duration)
        {
            var time =
                DateTime.Today
                    .Add(t)
                    .ToString(
                        "hh:mm tt",
                        CultureInfo.InvariantCulture);

            slots.Add(new TimeSlotViewModel
            {
                Time = time,
                IsBooked = booked.Contains(time)
            });
        }

        return slots;
    }
}
