
using System.ComponentModel.DataAnnotations;

namespace MediCareClinic.Models;

public class Appointment
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public Doctor? Doctor { get; set; }

    public string PatientId { get; set; } = string.Empty;

    public ApplicationUser? Patient { get; set; }

    [DataType(DataType.Date)]
    public DateTime AppointmentDate { get; set; }

    public string AppointmentTime { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public string? Notes { get; set; }
}
