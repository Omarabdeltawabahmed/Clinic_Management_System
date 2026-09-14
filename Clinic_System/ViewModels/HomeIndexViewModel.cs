using MediCareClinic.Models;

namespace MediCareClinic.ViewModels;

public class HomeIndexViewModel
{
    public int TotalDoctors { get; set; }
    public int TotalSpecialties { get; set; }
    public List<Doctor> FeaturedDoctors { get; set; } = new();
    public List<Specialty> Specialties { get; set; } = new();
}
