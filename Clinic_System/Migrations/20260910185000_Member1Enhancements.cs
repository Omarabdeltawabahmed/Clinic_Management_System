using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace MediCareClinic.Migrations
{
    [Migration("20260910185000_Member1Enhancements")]
    public partial class Member1Enhancements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(name: "BirthDate", table: "AspNetUsers", type: "datetime2", nullable: true);
            migrationBuilder.AddColumn<string>(name: "BloodType", table: "AspNetUsers", type: "nvarchar(10)", maxLength: 10, nullable: true);
            migrationBuilder.AddColumn<string>(name: "ChronicConditions", table: "AspNetUsers", type: "nvarchar(1000)", maxLength: 1000, nullable: true);
            migrationBuilder.AddColumn<string>(name: "Allergies", table: "AspNetUsers", type: "nvarchar(1000)", maxLength: 1000, nullable: true);
            migrationBuilder.AddColumn<string>(name: "Education", table: "Doctors", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Qualifications", table: "Doctors", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "Languages", table: "Doctors", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "ClinicRoom", table: "Doctors", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<int>(name: "VisitDurationMinutes", table: "Doctors", type: "int", nullable: false, defaultValue: 30);
            migrationBuilder.AddColumn<string>(name: "WorkingHours", table: "Doctors", type: "nvarchar(max)", nullable: true);
            migrationBuilder.AddColumn<string>(name: "BookingNumber", table: "Appointments", type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValueSql: "'MC-' + LEFT(REPLACE(CONVERT(varchar(36), NEWID()), '-', ''), 12)");
            migrationBuilder.AddColumn<DateTime>(name: "CreatedAt", table: "Appointments", type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()");
            migrationBuilder.CreateTable(name: "WorkingDays", columns: table => new { Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"), Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false) }, constraints: table => table.PrimaryKey("PK_WorkingDays", x => x.Id));
            migrationBuilder.CreateTable(name: "DoctorWorkingDays", columns: table => new { DoctorId = table.Column<int>(type: "int", nullable: false), WorkingDayId = table.Column<int>(type: "int", nullable: false), StartTime = table.Column<TimeSpan>(type: "time", nullable: false), EndTime = table.Column<TimeSpan>(type: "time", nullable: false) }, constraints: table => { table.PrimaryKey("PK_DoctorWorkingDays", x => new { x.DoctorId, x.WorkingDayId }); table.ForeignKey("FK_DoctorWorkingDays_Doctors_DoctorId", x => x.DoctorId, "Doctors", "Id", onDelete: ReferentialAction.Cascade); table.ForeignKey("FK_DoctorWorkingDays_WorkingDays_WorkingDayId", x => x.WorkingDayId, "WorkingDays", "Id", onDelete: ReferentialAction.Cascade); });
            migrationBuilder.CreateIndex(name: "IX_DoctorWorkingDays_WorkingDayId", table: "DoctorWorkingDays", column: "WorkingDayId");
            migrationBuilder.CreateIndex(name: "IX_Appointments_DoctorId_AppointmentDate_AppointmentTime", table: "Appointments", columns: new[] { "DoctorId", "AppointmentDate", "AppointmentTime" }, unique: true, filter: "[Status] <> 'Cancelled'");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Appointments_DoctorId_AppointmentDate_AppointmentTime", table: "Appointments");
            migrationBuilder.DropTable(name: "DoctorWorkingDays"); migrationBuilder.DropTable(name: "WorkingDays");
            migrationBuilder.DropColumn(name: "BookingNumber", table: "Appointments"); migrationBuilder.DropColumn(name: "CreatedAt", table: "Appointments");
            migrationBuilder.DropColumn(name: "BirthDate", table: "AspNetUsers"); migrationBuilder.DropColumn(name: "BloodType", table: "AspNetUsers"); migrationBuilder.DropColumn(name: "ChronicConditions", table: "AspNetUsers"); migrationBuilder.DropColumn(name: "Allergies", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "Education", table: "Doctors"); migrationBuilder.DropColumn(name: "Qualifications", table: "Doctors"); migrationBuilder.DropColumn(name: "Languages", table: "Doctors"); migrationBuilder.DropColumn(name: "ClinicRoom", table: "Doctors"); migrationBuilder.DropColumn(name: "VisitDurationMinutes", table: "Doctors"); migrationBuilder.DropColumn(name: "WorkingHours", table: "Doctors");
        }
    }
}
