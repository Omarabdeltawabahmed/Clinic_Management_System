using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Clinic_System.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int Id { get; set; }

        public int DoctorID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int SlotDurationMinutes { get; set; }


    }
}
