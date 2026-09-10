using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_System.Models
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

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}
