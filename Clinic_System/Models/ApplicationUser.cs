using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MediCareClinic.Models;

public class ApplicationUser : IdentityUser
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [StringLength(10)]
    public string? BloodType { get; set; }

    [StringLength(1000)]
    public string? ChronicConditions { get; set; }

    [StringLength(1000)]
    public string? Allergies { get; set; }
}
