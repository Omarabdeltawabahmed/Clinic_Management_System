using Clinic_System.Data;
using Clinic_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    public class DoctorLeavesController : Controller
    {
        private readonly AppDbContext _db;

        public DoctorLeavesController(AppDbContext db)
        {
            _db = db;
        }

        // عرض قائمة الإجازات المسجلة
        public async Task<IActionResult> Index()
        {
            var leaves = await _db.DoctorLeaves
                .ToListAsync();

            var doctors = await _db.Doctors.ToDictionaryAsync(d => d.Id, d => d.Name);
            ViewBag.Doctors = doctors;

            return View(leaves);
        }

        // إضافة إجازة جديدة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorLeave leave)
        {
            if (leave.DoctorId > 0 && leave.LeaveDate != default)
            {
                _db.DoctorLeaves.Add(leave);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // حذف إجازة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _db.DoctorLeaves.FindAsync(id);
            if (leave != null)
            {
                _db.DoctorLeaves.Remove(leave);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}