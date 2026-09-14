using System.ComponentModel.DataAnnotations;

namespace MediCareClinic.Models;

public class WorkingDay
{
    public int Id { get; set; }
    [Required, StringLength(20)] public string Name { get; set; } = string.Empty;
    public ICollection<DoctorWorkingDay> Doctors { get; set; } = new List<DoctorWorkingDay>();
}

public class DoctorWorkingDay
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    public int WorkingDayId { get; set; }
    public WorkingDay WorkingDay { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
