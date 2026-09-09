using MediCareClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var role in new[] { "Admin", "Doctor", "Patient" })
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        var adminEmail = "admin@medicare.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "MediCare Admin", EmailConfirmed = true };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!await db.Specialties.AnyAsync())
        {
            var cardiology = new Specialty { Name = "Cardiology", IconClass = "fa-solid fa-heart-pulse" };
            var dentistry = new Specialty { Name = "Dentistry", IconClass = "fa-solid fa-tooth" };
            var pediatrics = new Specialty { Name = "Pediatrics", IconClass = "fa-solid fa-baby" };
            var orthopedics = new Specialty { Name = "Orthopedics", IconClass = "fa-solid fa-bone" };
            db.Specialties.AddRange(cardiology, dentistry, pediatrics, orthopedics);
            await db.SaveChangesAsync();

            db.Doctors.AddRange(
                new Doctor { Name = "Dr. Salma Nasreen", SpecialtyId = cardiology.Id, ImageUrl = "/images/doctor-salma.jpg", Bio = "Consultant in cardiology with a patient-first approach and a focus on preventive care.", ConsultationFee = 220, Rating = 4.8, ReviewsCount = 94, YearsExperience = 11 },
                new Doctor { Name = "Dr. Karim Youssef", SpecialtyId = pediatrics.Id, ImageUrl = "/images/doctor-youssef.jpg", Bio = "Pediatric consultant experienced in child wellness, diagnosis and family-centered care.", ConsultationFee = 180, Rating = 4.7, ReviewsCount = 77, YearsExperience = 9 },
                new Doctor { Name = "Dr. Sara Ali", SpecialtyId = dentistry.Id, ImageUrl = "/images/doctor-sara.jpg", Bio = "Dental consultant providing restorative and preventive dental care for adults and children.", ConsultationFee = 200, Rating = 4.9, ReviewsCount = 121, YearsExperience = 10 },
                new Doctor { Name = "Dr. Ahmed Mahmoud", SpecialtyId = orthopedics.Id, ImageUrl = "/images/doctor-ahmed.jpg", Bio = "Orthopedic consultant specializing in joint, bone and sports-related conditions.", ConsultationFee = 250, Rating = 4.8, ReviewsCount = 106, YearsExperience = 13 }
            );
            await db.SaveChangesAsync();
        }
    }
}
