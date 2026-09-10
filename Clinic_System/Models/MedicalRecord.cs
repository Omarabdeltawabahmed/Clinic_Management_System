using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_System.Models
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }


        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }


        [Required(ErrorMessage = "Please enter the patient's complaint.")]
        [Display(Name = "Chief Complaint")]
        public string Complaint { get; set; } = string.Empty;


        [Required(ErrorMessage = "Diagnosis is required.")]
        public string Diagnosis { get; set; } = string.Empty;


        [Required(ErrorMessage = "Prescription details are required.")]
        [DataType(DataType.MultilineText)]
        public string Prescription { get; set; } = string.Empty;


        public string? AttachmentPath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
