using Clinic_System.Data;
using Clinic_System.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    public class WorkingHoursController : Controller
    {
        private readonly AppDbContext _db;
        public WorkingHoursController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var workingHours = await _db.workingHours.Include(d => d.Doctor).ToListAsync();
            return View(workingHours);
        }

        // GET
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_db.Doctors, "Id", "Name");
            return View();
        }

        // POST
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Day, StartTime, EndTime, DoctorId")] WorkingHour wh)
        {
            if(ModelState.IsValid)
            {
                _db.Add(wh);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DoctorId"] = new SelectList(_db.Doctors, "Id", "Name", wh.DoctorId);
            return View(wh);
        }


        // GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var wh = await _db.workingHours.FindAsync(id);
            if(wh == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_db.Doctors, "Id", "Name", wh.DoctorId);
            return View(wh);
        }

        // POST
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , [Bind("Id, Day, StartTime, EndTime, DoctorId")] WorkingHour wh)
        {
            if (id != wh.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(wh);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.workingHours.Any(w => w.Id == id))
                    {
                        return NotFound(); 
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_db.Doctors, "Id", "Name", wh.DoctorId);
            return View(wh);
        }


        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var wh = await _db.workingHours.Include(d => d.Doctor).FirstOrDefaultAsync(w => w.Id == id);
            if(wh == null)
            {
                return NotFound();
            }
            return View(wh);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wh = await _db.workingHours.FindAsync(id);
            if (wh != null)
            {
                _db.workingHours.Remove(wh);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
