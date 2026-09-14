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
            if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));

        var adminEmail = "admin@medicare.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "MediCare Admin", EmailConfirmed = true };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!await db.WorkingDays.AnyAsync())
        {
            db.WorkingDays.AddRange(new[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday" }.Select(Name => new WorkingDay { Name = Name }));
            await db.SaveChangesAsync();
        }

        var specialtySeed = new (string Name, string Icon)[] {
            ("Cardiology", "fa-solid fa-heart-pulse"), ("Dentistry", "fa-solid fa-tooth"), ("Pediatrics", "fa-solid fa-baby"), ("Orthopedics", "fa-solid fa-bone"),
            ("Dermatology", "fa-solid fa-hand-dots"), ("Neurology", "fa-solid fa-brain"), ("Ophthalmology", "fa-solid fa-eye"), ("Internal Medicine", "fa-solid fa-stethoscope")
        };
        foreach (var item in specialtySeed)
            if (!await db.Specialties.AnyAsync(x => x.Name == item.Name)) db.Specialties.Add(new Specialty { Name = item.Name, IconClass = item.Icon });
        await db.SaveChangesAsync();

        if (await db.Doctors.CountAsync() < 24)
        {
            var specs = await db.Specialties.OrderBy(x => x.Id).ToListAsync();
            var images = new[] { "/images/doctor-salma.jpg", "/images/doctor-youssef.jpg", "/images/doctor-sara.jpg", "/images/doctor-ahmed.jpg" };
            var names = new[] { "Dr. Salma Nasreen", "Dr. Karim Youssef", "Dr. Sara Ali", "Dr. Ahmed Mahmoud", "Dr. Omar Hassan", "Dr. Lina Samir", "Dr. Hany Adel", "Dr. Mariam Nabil", "Dr. Tarek Mostafa", "Dr. Nour Khaled", "Dr. Yara Emad", "Dr. Adam Fathy", "Dr. Reem Ashraf", "Dr. Ziad Ahmed", "Dr. Jana Wael", "Dr. Mahmoud Sherif", "Dr. Farah Amr", "Dr. Khaled Sami", "Dr. Menna Hossam", "Dr. Seif Ibrahim", "Dr. Aya Mostafa", "Dr. Ahmed Tamer", "Dr. Huda Essam", "Dr. Youmna Adel" };
            var educations = new[] { "Cairo University Faculty of Medicine", "Ain Shams University Faculty of Medicine", "Alexandria University Faculty of Medicine" };
            for (int i = await db.Doctors.CountAsync(); i < 24; i++)
            {
                var d = new Doctor
                {
                    Name = names[i], SpecialtyId = specs[i % specs.Count].Id, ImageUrl = images[i % images.Length],
                    Bio = "Experienced consultant committed to evidence-based care, clear communication and a comfortable patient experience.",
                    Education = educations[i % educations.Length], Qualifications = "MD, Board Certified Specialist; Professional Clinical Certification",
                    Languages = "Arabic, English", ClinicRoom = $"Room {101 + i}", VisitDurationMinutes = i % 3 == 0 ? 30 : 20,
                    WorkingHours = i % 2 == 0 ? "09:00 AM – 03:00 PM" : "02:00 PM – 08:00 PM",
                    ConsultationFee = 180 + (i % 8) * 15, Rating = 4.5 + (i % 5) * .1, ReviewsCount = 45 + i * 7, YearsExperience = 5 + (i % 12), IsActive = true
                };
                db.Doctors.Add(d);
            }
            await db.SaveChangesAsync();
        }

        var allDoctors = await db.Doctors.ToListAsync();
        foreach (var d in allDoctors)
        {
            d.Education ??= d.Id % 2 == 0 ? "Cairo University Faculty of Medicine" : "Ain Shams University Faculty of Medicine";
            d.Qualifications ??= "MD, Board Certified Specialist; Professional Clinical Certification";
            d.Languages ??= "Arabic, English"; d.ClinicRoom ??= $"Room {100 + d.Id}";
            if (d.VisitDurationMinutes <= 0) d.VisitDurationMinutes = 30;
            d.WorkingHours ??= d.Id % 2 == 0 ? "09:00 AM – 03:00 PM" : "02:00 PM – 08:00 PM";
        }
        await db.SaveChangesAsync();

        if (!await db.DoctorWorkingDays.AnyAsync())
        {
            var days = await db.WorkingDays.ToListAsync();
            var doctors = await db.Doctors.ToListAsync();
            foreach (var d in doctors)
            {
                foreach (var day in days.Where((_, idx) => (idx + d.Id) % 3 != 0))
                    db.DoctorWorkingDays.Add(new DoctorWorkingDay { DoctorId = d.Id, WorkingDayId = day.Id, StartTime = d.Id % 2 == 0 ? new TimeSpan(9, 0, 0) : new TimeSpan(14, 0, 0), EndTime = d.Id % 2 == 0 ? new TimeSpan(15, 0, 0) : new TimeSpan(20, 0, 0) });
            }
            await db.SaveChangesAsync();
        }
    }
}
