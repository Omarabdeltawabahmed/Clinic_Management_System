using System.Collections.Generic;

namespace Clinic_System.ViewModels
{
    public class ReportsViewModel
    {
        // الكروت العلوية (KPIs)
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int WaitingAppointments { get; set; }
        public decimal TotalRevenue { get; set; }

        // بيانات الرسم البياني للإيرادات حسب التخصص (Income Per Specialty)
        public List<string> SpecialtyNames { get; set; } = new();
        public List<decimal> SpecialtyRevenues { get; set; } = new();

        // بيانات الرسم البياني للحجوزات خلال أيام الأسبوع (Visits Per Day)
        public List<string> DaysOfWeek { get; set; } = new();
        public List<int> DailyAppointments { get; set; } = new();
    }
}