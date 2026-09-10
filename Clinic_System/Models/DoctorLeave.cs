using System.ComponentModel.DataAnnotations;

namespace Clinic_System.Models
{
    public class DoctorLeave
    {
        [Key]
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public DateTime LeaveDate { get; set; }

        public string Reason { get; set; } = "";
    }
}
