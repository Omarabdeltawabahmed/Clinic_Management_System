namespace MediCareClinic.Models;

public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IconClass { get; set; } = "fa-solid fa-stethoscope";
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
