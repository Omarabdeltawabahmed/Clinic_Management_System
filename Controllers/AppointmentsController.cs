using MediCareClinic.Data;
using MediCareClinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Controllers;

[Authorize]
public class AppointmentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public AppointmentsController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context; _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int doctorId)
    {
        var doctor = await _context.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(d => d.Id == doctorId);
        if (doctor == null) return NotFound();
        ViewBag.Doctor = doctor;
        return View(new Appointment { DoctorId = doctorId, AppointmentDate = DateTime.Today.AddDays(1), AppointmentTime = "10:00 AM" });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Appointment model)
    {
        var doctor = await _context.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(d => d.Id == model.DoctorId);
        if (doctor == null) return NotFound();
        if (model.AppointmentDate.Date < DateTime.Today)
            ModelState.AddModelError(nameof(model.AppointmentDate), "Please choose today or a future date.");
        if (!ModelState.IsValid) { ViewBag.Doctor = doctor; return View(model); }

        model.PatientId = _userManager.GetUserId(User)!;
        model.Status = "Pending";
        _context.Appointments.Add(model);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Appointment booked successfully.";
        return RedirectToAction(nameof(MyAppointments));
    }

    public async Task<IActionResult> MyAppointments()
    {
        var id = _userManager.GetUserId(User);
        var items = await _context.Appointments.Include(a => a.Doctor).ThenInclude(d => d.Specialty)
            .Where(a => a.PatientId == id).OrderByDescending(a => a.AppointmentDate).ToListAsync();
        return View(items);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = _userManager.GetUserId(User);
        var appt = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id && a.PatientId == userId);
        if (appt != null) { appt.Status = "Cancelled"; await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(MyAppointments));
    }
}