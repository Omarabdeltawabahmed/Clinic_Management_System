using MediCareClinic.Models;
namespace MediCareClinic.ViewModels;
public class DoctorProfileViewModel
{
    public Doctor Doctor { get; set; } = null!;
    public List<DoctorWorkingDay> WorkingDays { get; set; } = new();
}
