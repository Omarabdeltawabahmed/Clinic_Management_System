using System.ComponentModel.DataAnnotations;

namespace Clinic_System.ViewModels
{
    public class CreateWalkInViewModel
    {
        [Required(ErrorMessage = "Patient name is required.")]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PatientPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please select appointment time.")]
        [DataType(DataType.Time)]
        [Display(Name = "Time")]
        public TimeSpan Time { get; set; }
    }
}
