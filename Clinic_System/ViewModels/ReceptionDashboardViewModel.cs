using Clinic_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Clinic_System.ViewModels
{
    public class ReceptionDashboardViewModel
    {
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public int? SelectedDoctorId { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; } = new List<Doctor>();
        public IEnumerable<AppointmentItemViewModel> Appointments { get; set; } = new List<AppointmentItemViewModel>();
    }
}
