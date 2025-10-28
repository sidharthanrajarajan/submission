using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UAMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitUserNameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "training",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "training",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "training",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                schema: "training",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "training",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "training",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                schema: "training",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "training",
                table: "Users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
