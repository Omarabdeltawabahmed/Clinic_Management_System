using MediCareClinic.Data;
using MediCareClinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Controllers;
public class HomeController : Controller
{
    private readonly AppDbContext _context;
    public HomeController(AppDbContext context) => _context = context;
    public async Task<IActionResult> Index() => View(new HomeIndexViewModel { TotalDoctors = await _context.Doctors.CountAsync(d => d.IsActive), TotalSpecialties = await _context.Specialties.CountAsync(), FeaturedDoctors = await _context.Doctors.Include(d => d.Specialty).Where(d => d.IsActive).OrderByDescending(d => d.Rating).Take(4).ToListAsync(), Specialties = await _context.Specialties.Include(s => s.Doctors).ToListAsync() });
    public IActionResult Error() => View("Error");
    public IActionResult StatusCode(int code) => View("StatusCode", code);
}
