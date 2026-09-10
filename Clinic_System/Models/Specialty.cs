using System.ComponentModel.DataAnnotations;

namespace Clinic_System.Models
{
    public class Specialty
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public string? IconPath { get; set; }

        public ICollection<Doctor> doctors { get; set; } = new List<Doctor>();
    }
}
