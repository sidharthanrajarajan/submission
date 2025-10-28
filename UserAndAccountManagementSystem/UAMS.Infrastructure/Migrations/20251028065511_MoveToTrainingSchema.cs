using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UAMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveToTrainingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "training");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "auth",
                newName: "Users",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "auth",
                newName: "UserRoles",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "Transactions",
                schema: "account",
                newName: "Transactions",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "auth",
                newName: "Roles",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                schema: "auth",
                newName: "RolePermissions",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "Permissions",
                schema: "auth",
                newName: "Permissions",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "Branches",
                schema: "bank",
                newName: "Branches",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "Banks",
                schema: "bank",
                newName: "Banks",
                newSchema: "training");

            migrationBuilder.RenameTable(
                name: "Accounts",
                schema: "account",
                newName: "Accounts",
                newSchema: "training");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.EnsureSchema(
                name: "bank");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "training",
                newName: "Users",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "training",
                newName: "UserRoles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "Transactions",
                schema: "training",
                newName: "Transactions",
                newSchema: "account");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "training",
                newName: "Roles",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                schema: "training",
                newName: "RolePermissions",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "Permissions",
                schema: "training",
                newName: "Permissions",
                newSchema: "auth");

            migrationBuilder.RenameTable(
                name: "Branches",
                schema: "training",
                newName: "Branches",
                newSchema: "bank");

            migrationBuilder.RenameTable(
                name: "Banks",
                schema: "training",
                newName: "Banks",
                newSchema: "bank");

            migrationBuilder.RenameTable(
                name: "Accounts",
                schema: "training",
                newName: "Accounts",
                newSchema: "account");
        }
    }
}
