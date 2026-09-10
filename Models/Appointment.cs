using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public string DoctorName { get; set; } = string.Empty;

        public DateTime AppointmentDate { get; set; } = DateTime.Now;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Waiting;

        public virtual MedicalRecord? MedicalRecord { get; set; }
    }
}