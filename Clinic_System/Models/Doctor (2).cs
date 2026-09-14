using System.ComponentModel.DataAnnotations;

namespace MediCareClinic.Models;

public class Doctor
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string Name { get; set; } = string.Empty;
    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
    public string? Education { get; set; }
    public string? Qualifications { get; set; }
    public string? Languages { get; set; }
    public string? ClinicRoom { get; set; }
    public int VisitDurationMinutes { get; set; } = 30;
    public string? WorkingHours { get; set; }
    public decimal ConsultationFee { get; set; }
    public double Rating { get; set; }
    public int ReviewsCount { get; set; }
    public int YearsExperience { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<DoctorWorkingDay> WorkingDays { get; set; } = new List<DoctorWorkingDay>();
}
