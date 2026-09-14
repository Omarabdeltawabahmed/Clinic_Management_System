using System.ComponentModel.DataAnnotations;

namespace MediCareClinic.Models;

public class Appointment
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public ApplicationUser? Patient { get; set; }
    [DataType(DataType.Date), Required] public DateTime AppointmentDate { get; set; }
    [Required] public string AppointmentTime { get; set; } = string.Empty;
    [Required, StringLength(20)] public string Status { get; set; } = "Pending";
    [StringLength(1000)] public string? Notes { get; set; }
    [Required, StringLength(30)] public string BookingNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
