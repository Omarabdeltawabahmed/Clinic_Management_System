using MediCareClinic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Controllers;

public class DoctorsController : Controller
{
    private readonly AppDbContext _context;
    public DoctorsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index(int? specialtyId)
    {
        var query = _context.Doctors.Include(d => d.Specialty).Where(d => d.IsActive).AsQueryable();
        if (specialtyId.HasValue) query = query.Where(d => d.SpecialtyId == specialtyId.Value);
        ViewBag.Specialties = await _context.Specialties.ToListAsync();
        ViewBag.SelectedSpecialty = specialtyId;
        return View(await query.OrderByDescending(d => d.Rating).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var doctor = await _context.Doctors.Include(d => d.Specialty).FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
        return doctor == null ? NotFound() : View(doctor);
    }
}
