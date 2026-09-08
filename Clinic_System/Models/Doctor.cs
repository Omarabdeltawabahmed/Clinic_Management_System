using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_System.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Doctor Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Doctor Name")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Salary is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 1000000, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ProfilePictureUrl { get; set; }


        [Required(ErrorMessage = "Please select a specialty.")]
        [Display(Name = "Specialty")]
        [ForeignKey("Specialty")]
        public int SpecialtyId { get; set; }

        public Specialty? Specialty { get; set; }

    }
}
