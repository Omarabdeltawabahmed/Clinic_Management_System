using Clinic_System.Data;
using Clinic_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic_System.Controllers
{
    public class SpecialtiesController : Controller
    {

        private readonly AppDbContext _db;
        public SpecialtiesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var specialists = await _db.Specialties.ToListAsync();
            return View(specialists);
        }

        // GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var specialist = await _db.Specialties.FirstOrDefaultAsync(x => x.Id == id);
            if (specialist == null)
            {
                return NotFound();
            }
            return View(specialist);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST 
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IconPath")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                _db.Add(specialty);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }


        // GET 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var specialist = await _db.Specialties.FirstAsync(x => x.Id == id);
            if(specialist == null)
            {
                return NotFound();
            }
            return View(specialist);
        }


        // POST
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , [Bind("Id ,Name ,Description, IconPath")] Specialty specialty)
        {
            if (id != specialty.Id) return NotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    _db.Update(specialty);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Specialties.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specialty);
        }


        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var specialty = await _db.Specialties.FirstOrDefaultAsync(m => m.Id == id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var specialty = await _db.Specialties.FindAsync(id);
            if(specialty != null)
            {
                _db.Specialties.Remove(specialty);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
