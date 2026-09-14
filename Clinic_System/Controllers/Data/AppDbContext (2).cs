using MediCareClinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediCareClinic.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<WorkingDay> WorkingDays => Set<WorkingDay>();
    public DbSet<DoctorWorkingDay> DoctorWorkingDays => Set<DoctorWorkingDay>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<DoctorWorkingDay>().HasKey(x => new { x.DoctorId, x.WorkingDayId });
        builder.Entity<DoctorWorkingDay>().HasOne(x => x.Doctor).WithMany(x => x.WorkingDays).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<DoctorWorkingDay>().HasOne(x => x.WorkingDay).WithMany(x => x.Doctors).HasForeignKey(x => x.WorkingDayId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Appointment>().HasIndex(x => new { x.DoctorId, x.AppointmentDate, x.AppointmentTime }).IsUnique().HasFilter("[Status] <> 'Cancelled'");
        builder.Entity<Doctor>().Property(x => x.ConsultationFee).HasPrecision(18, 2);
    }
}
