using Clinic_System.Models;

namespace Clinic_System.ViewModels
{
    public class AppointmentItemViewModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;
        public TimeSpan Time { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
