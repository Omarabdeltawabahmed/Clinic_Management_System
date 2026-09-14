using MediCareClinic.Data;
using MediCareClinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Controllers;

public class DoctorsController : Controller
{
    private readonly AppDbContext _context;
    public DoctorsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search, int? specialtyId, string? day, decimal? maxFee, string sort = "rating", int page = 1)
    {
        const int pageSize = 8;
        var query = _context.Doctors.Include(d => d.Specialty).Include(d => d.WorkingDays).ThenInclude(x => x.WorkingDay).Where(d => d.IsActive).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(d => d.Name.Contains(search) || d.Specialty.Name.Contains(search));
        if (specialtyId.HasValue) query = query.Where(d => d.SpecialtyId == specialtyId.Value);
        if (!string.IsNullOrWhiteSpace(day)) query = query.Where(d => d.WorkingDays.Any(x => x.WorkingDay.Name == day));
        if (maxFee.HasValue) query = query.Where(d => d.ConsultationFee <= maxFee.Value);
        query = sort switch
        {
            "fee-low" => query.OrderBy(d => d.ConsultationFee),
            "fee-high" => query.OrderByDescending(d => d.ConsultationFee),
            "experience" => query.OrderByDescending(d => d.YearsExperience),
            "name" => query.OrderBy(d => d.Name),
            _ => query.OrderByDescending(d => d.Rating).ThenByDescending(d => d.ReviewsCount)
        };
        var total = await query.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        page = Math.Clamp(page, 1, totalPages);
        var doctors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return View(new DoctorCatalogViewModel { Doctors = doctors, Specialties = await _context.Specialties.OrderBy(x => x.Name).ToListAsync(), AvailableDays = await _context.WorkingDays.OrderBy(x => x.Id).Select(x => x.Name).ToListAsync(), Search = search, SpecialtyId = specialtyId, Day = day, MaxFee = maxFee, Sort = sort, Page = page, TotalPages = totalPages });
    }

    public async Task<IActionResult> Details(int id)
    {
        var doctor = await _context.Doctors.Include(d => d.Specialty).Include(d => d.WorkingDays).ThenInclude(x => x.WorkingDay).FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
        return doctor == null ? NotFound() : View(new DoctorProfileViewModel { Doctor = doctor, WorkingDays = doctor.WorkingDays.OrderBy(x => x.WorkingDayId).ToList() });
    }
}
