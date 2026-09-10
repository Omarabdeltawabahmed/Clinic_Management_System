using System.ComponentModel.DataAnnotations;

namespace Clinic_System.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        [StringLength(15)]
        public string Phone { get; set; } = "";

        [StringLength(200)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
