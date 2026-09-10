using Clinic_System.Data;
using Clinic_System.Models;
using Clinic_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly AppDbContext _db;

        public ReportsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _db.Appointments.ToListAsync();
            var doctors = await _db.Doctors.Include(d => d.Specialty).ToListAsync();

            var total = appointments.Count;
            var completed = appointments.Count(a => a.Status == AppointmentStatus.Done);
            var cancelled = appointments.Count(a => a.Status == AppointmentStatus.Cancelled || a.Status == AppointmentStatus.NoShow);
            var waiting = appointments.Count(a => a.Status == AppointmentStatus.Waiting || a.Status == AppointmentStatus.InProgress);

            // 1. خريطة لربط اسم الدكتور بسعر كشفه الفعلي
            var doctorFeeMap = doctors.ToDictionary(d => d.Name, d => d.Fee);

            decimal totalRevenue = 0;
            var specialtyRevenueDict = new Dictionary<string, decimal>();

            // 2. حساب الإيراد الفعلي للكشوفات المكتملة بناءً على دكتور كل حجز
            foreach (var app in appointments.Where(a => a.Status == AppointmentStatus.Done))
            {
                var doc = doctors.FirstOrDefault(d => d.Name == app.DoctorName);
                decimal fee = doc?.Fee ?? 0m; // سعر كشف الدكتور الحقيقي

                totalRevenue += fee;

                string specName = doc?.Specialty?.Name ?? "General";
                if (!specialtyRevenueDict.ContainsKey(specName))
                    specialtyRevenueDict[specName] = 0;

                specialtyRevenueDict[specName] += fee;
            }

            var dailyData = appointments
                .GroupBy(a => a.AppointmentDate.DayOfWeek)
                .Select(g => new { Day = g.Key.ToString(), Count = g.Count() })
                .ToList();

            var viewModel = new ReportsViewModel
            {
                TotalAppointments = total,
                CompletedAppointments = completed,
                CancelledAppointments = cancelled,
                WaitingAppointments = waiting,
                TotalRevenue = totalRevenue,
                SpecialtyNames = specialtyRevenueDict.Keys.ToList(),
                SpecialtyRevenues = specialtyRevenueDict.Values.ToList(),
                DaysOfWeek = dailyData.Select(d => d.Day).ToList(),
                DailyAppointments = dailyData.Select(d => d.Count).ToList()
            };

            return View(viewModel);
        }
    }
}