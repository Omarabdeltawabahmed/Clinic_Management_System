using Microsoft.AspNetCore.Identity;

namespace Clinic_System.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. إنشاء الأدوار (Roles)
            string[] roles = { "Admin", "Receptionist", "Doctor", "Patient" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }



            // 2. إنشاء حساب Admin افتراضي
            string adminEmail = "admin@clinic.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }



            // 3. إنشاء حساب Receptionist بكلمة السر الجديدة
            string recepEmail = "reception@clinic.com";
            var recepUser = await userManager.FindByEmailAsync(recepEmail);

            if (recepUser == null)
            {
                recepUser = new IdentityUser
                {
                    UserName = recepEmail,
                    Email = recepEmail,
                    EmailConfirmed = true
                };

                // اكتب كلمة السر الجديدة التي تريدها هنا (مثل: Recep@123)
                var result = await userManager.CreateAsync(recepUser, "Recep@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(recepUser, "Receptionist");
                }
            }
        }
    }
}