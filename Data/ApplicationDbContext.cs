using Microsoft.EntityFrameworkCore;
using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<MedicalRecord>()
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}