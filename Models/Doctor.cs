namespace MediCareClinic.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
    public decimal ConsultationFee { get; set; }
    public double Rating { get; set; }
    public int ReviewsCount { get; set; }
    public int YearsExperience { get; set; }
    public bool IsActive { get; set; } = true;
}

