using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_System.Models
{
    public enum DayOfWeek
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Weednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7,
    }

    public class WorkingHour
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Day of the week is required.")]
        [Display(Name = "Day")]
        public DayOfWeek Day { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required .")]
        [DataType (DataType.Time)]
        [Display(Name ="End Time")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Please select a doctor.")]
        [Display(Name = "Doctor")]
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }

    }
}
