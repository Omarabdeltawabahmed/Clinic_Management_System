using MediCareClinic.Models;

namespace MediCareClinic.ViewModels;

public class DoctorCatalogViewModel
{
    public List<Doctor> Doctors { get; set; } = new();
    public List<Specialty> Specialties { get; set; } = new();
    public List<string> AvailableDays { get; set; } = new();
    public string? Search { get; set; }
    public int? SpecialtyId { get; set; }
    public string? Day { get; set; }
    public decimal? MaxFee { get; set; }
    public string Sort { get; set; } = "rating";
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; }
}
