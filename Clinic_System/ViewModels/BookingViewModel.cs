using System.ComponentModel.DataAnnotations;
using MediCareClinic.Models;

namespace MediCareClinic.ViewModels;

public class BookingViewModel
{
    public int DoctorId { get; set; }

    public Doctor? Doctor { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public string AppointmentTime { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Notes { get; set; }

    public List<AvailableDateViewModel> AvailableDates { get; set; } = new();

    public List<TimeSlotViewModel> Slots { get; set; } = new();
}

public class AvailableDateViewModel
{
    public DateTime Date { get; set; }

    public string DisplayName { get; set; } = string.Empty;
}

public class TimeSlotViewModel
{
    public string Time { get; set; } = string.Empty;

    public bool IsBooked { get; set; }
}