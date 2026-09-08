using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_System.Migrations
{
    /// <inheritdoc />
    public partial class updateDoctorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biography",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Doctors",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Doctors",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ConsultationFee",
                table: "Doctors",
                newName: "Salary");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Doctors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Doctors",
                newName: "ConsultationFee");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Doctors",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Doctors",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
