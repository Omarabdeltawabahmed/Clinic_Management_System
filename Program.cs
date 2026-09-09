using Microsoft.EntityFrameworkCore;
using ClinicManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// إضافة الـ ConnectionString والـ DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
// Seed Data للـ Testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClinicManagementSystem.Data.ApplicationDbContext>();
    if (!context.Appointments.Any())
    {
        context.Appointments.AddRange(
            new ClinicManagementSystem.Models.Appointment 
            { 
                PatientName = "أحمد محمود", 
                DoctorName = "د. علي", 
                AppointmentDate = DateTime.Today.AddHours(10), 
                Status = ClinicManagementSystem.Models.AppointmentStatus.Waiting 
            },
            new ClinicManagementSystem.Models.Appointment 
            { 
                PatientName = "سارة محمد", 
                DoctorName = "د. علي", 
                AppointmentDate = DateTime.Today.AddHours(11), 
                Status = ClinicManagementSystem.Models.AppointmentStatus.InProgress 
            }
        );
        context.SaveChanges();
    }
}
app.Run();