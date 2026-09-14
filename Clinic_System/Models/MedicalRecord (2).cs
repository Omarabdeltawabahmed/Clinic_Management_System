namespace MediCareClinic.Models;

public class MedicalRecord
{
    public int Id { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
