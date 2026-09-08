using Clinic_System.Data;
using Clinic_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly AppDbContext _db;
        public DoctorsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _db.Doctors.Include(d => d.Specialty).ToListAsync();
            return View(doctors);
        }

        // GET
        public IActionResult Create()
        {
            ViewData["SpecialtyId"] = new SelectList(_db.Specialties, "Id", "Name");
            return View();
        }

        // POST
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Salary,ProfilePictureUrl,SpecialtyId")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _db.Add(doctor);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SpecialtyId"] = new SelectList(_db.Specialties, "Id", "Name", doctor.SpecialtyId);
            return View(doctor);
        }


        // GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var doctor = await _db.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            ViewData["SpecialtyId"] = new SelectList(_db.Specialties, "Id", "Name", doctor.SpecialtyId);
            return View(doctor);
        }

        // POST
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Salary,ProfilePictureUrl,SpecialtyId")] Doctor doctor)
        {
            if (id != doctor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(doctor);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Doctors.Any(d => d.Id == id))
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SpecialtyId"] = new SelectList(_db.Specialties, "Id", "Name", doctor.SpecialtyId);
            return View(doctor);
        }

        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var doctor = await _db.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _db.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _db.Doctors.Remove(doctor);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
