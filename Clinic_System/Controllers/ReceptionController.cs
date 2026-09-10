using Clinic_System.Data;
using Clinic_System.Models;
using Clinic_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    [Authorize(Roles = "Admin,Receptionist")]
    public class ReceptionController : Controller
    {
        private readonly AppDbContext _db;

        public ReceptionController(AppDbContext db)
        {
            _db = db;
        }

        // GET
        public async Task<IActionResult> Index(DateTime? date, int? doctorId)
        {
            var filterDate = date ?? DateTime.Today;

            var doctors = await _db.Doctors
                .Include(d => d.Specialty)
                .ToListAsync();

            var query = _db.Appointments
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDate.Date == filterDate.Date);

            if (doctorId.HasValue)
            {
                var selectedDoctor = doctors.FirstOrDefault(d => d.Id == doctorId.Value);
                if (selectedDoctor != null)
                {
                    query = query.Where(a => a.DoctorName == selectedDoctor.Name);
                }
            }

            var appointmentsList = await query.OrderBy(a => a.AppointmentDate).ToListAsync();

            var appointments = appointmentsList.Select(a => {
                var doctorObj = doctors.FirstOrDefault(d => d.Name == a.DoctorName);
                return new AppointmentItemViewModel
                {
                    Id = a.Id,
                    PatientName = a.Patient != null ? a.Patient.Name : a.PatientName,
                    PatientPhone = a.Patient != null ? a.Patient.Phone : "N/A",
                    DoctorName = a.DoctorName,
                    SpecialtyName = doctorObj?.Specialty?.Name ?? "General",
                    Time = a.AppointmentDate.TimeOfDay,
                    Status = a.Status
                };
            }).ToList();

            var viewModel = new ReceptionDashboardViewModel
            {
                SelectedDate = filterDate,
                SelectedDoctorId = doctorId,
                Doctors = doctors,
                Appointments = appointments
            };

            // التعديل هنا لدمج التخصص مع اسم الدكتور
            ViewBag.DoctorsList = new SelectList(
                doctors.Select(d => new {
                    Id = d.Id,
                    NameWithSpecialty = $"Dr. {d.Name} ({(d.Specialty != null ? d.Specialty.Name : "General")})"
                }),
                "Id",
                "NameWithSpecialty",
                doctorId
            );

            return View(viewModel);
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int appointmentId, AppointmentStatus newStatus)
        {
            var appointment = await _db.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.Status = newStatus;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWalkIn(CreateWalkInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = await _db.Doctors.FindAsync(model.DoctorId);
                string doctorName = doctor != null ? doctor.Name : "Unknown Doctor";

                var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Phone == model.PatientPhone);

                if (patient == null)
                {
                    patient = new Patient
                    {
                        Name = model.PatientName,
                        Phone = model.PatientPhone,
                        CreatedAt = DateTime.Now
                    };
                    _db.Patients.Add(patient);
                    await _db.SaveChangesAsync();
                }

                DateTime combinedDateTime = DateTime.Today.Add(model.Time);

                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    PatientName = patient.Name,
                    DoctorName = doctorName,
                    AppointmentDate = combinedDateTime,
                    Status = AppointmentStatus.Waiting
                };

                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

    }
}