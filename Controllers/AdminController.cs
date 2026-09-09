using MediCareClinic.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    public AdminController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        ViewBag.Doctors = await _context.Doctors.CountAsync();
        ViewBag.Specialties = await _context.Specialties.CountAsync();
        ViewBag.Appointments = await _context.Appointments.CountAsync();
        ViewBag.Pending = await _context.Appointments.CountAsync(a => a.Status == "Pending");
        return View();
    }
}
