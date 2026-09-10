using Clinic_System.Data;
using Clinic_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    public class DoctorController : Controller
    {
        private readonly AppDbContext _db;

        public DoctorController(AppDbContext db)
        {
            _db = db;
        }

        // 1. عرض قائمة الكشوفات اليومية
        public async Task<IActionResult> Index(string? doctorName)
        {
            var today = DateTime.Today;

            var query = _db.Appointments
                .Include(a => a.Patient)
                .Include(a => a.MedicalRecord)
                .Where(a => a.AppointmentDate.Date == today &&
                            a.Status != AppointmentStatus.Cancelled);

            if (!string.IsNullOrEmpty(doctorName))
            {
                query = query.Where(a => a.DoctorName == doctorName);
            }

            var appointments = await query
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            ViewBag.Doctors = await _db.Doctors.ToListAsync() ?? new List<Doctor>();
            ViewBag.SelectedDoctor = doctorName;

            return View(appointments);
        }

        // 2. زرار الـ Start: يغير حالة الحجز لـ InProgress
        [HttpPost]
        public async Task<IActionResult> StartConsultation(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.InProgress;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 3. زرار الـ No Show
        [HttpPost]
        public async Task<IActionResult> MarkNoShow(int id)
        {
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.NoShow;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 4. صفحة كشف / روشتة المريض (GET)
        public async Task<IActionResult> CreateRecord(int appointmentId)
        {
            var appointment = await _db.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.PatientName = appointment.Patient != null ? appointment.Patient.Name : appointment.PatientName;
            ViewBag.AppointmentId = appointmentId;

            // لو فيه سجل قديم يعرضه للتعديل، لو مفيش يفتح سجل جديد
            var existingRecord = await _db.MedicalRecords.FirstOrDefaultAsync(m => m.AppointmentId == appointmentId);

            if (existingRecord != null)
            {
                return View(existingRecord);
            }

            var newRecord = new MedicalRecord
            {
                AppointmentId = appointmentId
            };

            return View(newRecord);
        }

        // 5. حفظ الروشتة وتغيير الحالة لـ Done (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecord(MedicalRecord model)
        {
            ModelState.Remove("Appointment");
            ModelState.Remove("Patient");

            if (ModelState.IsValid)
            {
                var existingRecord = await _db.MedicalRecords
                    .FirstOrDefaultAsync(m => m.AppointmentId == model.AppointmentId);

                if (existingRecord != null)
                {
                    existingRecord.Complaint = model.Complaint;
                    existingRecord.Diagnosis = model.Diagnosis;
                    existingRecord.Prescription = model.Prescription;
                    _db.MedicalRecords.Update(existingRecord);
                }
                else
                {
                    _db.MedicalRecords.Add(model);
                }

                // تغيير حالة الحجز لـ Done فوراً
                var appointment = await _db.Appointments.FindAsync(model.AppointmentId);
                if (appointment != null)
                {
                    appointment.Status = AppointmentStatus.Done;
                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var app = await _db.Appointments.Include(a => a.Patient).FirstOrDefaultAsync(a => a.Id == model.AppointmentId);
            ViewBag.PatientName = app?.Patient != null ? app.Patient.Name : app?.PatientName;
            ViewBag.AppointmentId = model.AppointmentId;

            return View(model);
        }
    }
}