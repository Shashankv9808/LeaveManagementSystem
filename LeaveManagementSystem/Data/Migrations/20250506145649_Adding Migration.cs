using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ffad0b4-16ae-4493-9fdf-7c027680e719\r\n", null, "Supervisor", "SUPERVISOR" },
                    { "7b9f7fc2-38b5-484f-aaef-ce52333cf198", null, "Employee", "EMPLOYEE" },
                    { "7f699909-5287-412c-87fc-244531eaa8c7", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3c25edb0-e14e-46ea-a155-401cd22ba74e", 0, "a1958805-7e96-4673-a3df-cc6517ee78a8", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", null, "AQAAAAIAAYagAAAAEHXDybl/Y1ksTHKJHc/2GNSMEm7rMzUZWl8oqkUhjl0dnHNssZLXD3o3788GCBARjg==", null, false, "82ca149b-e64c-4080-9e53-0c32e04c105f", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7b9f7fc2-38b5-484f-aaef-ce52333cf198", "3c25edb0-e14e-46ea-a155-401cd22ba74e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ffad0b4-16ae-4493-9fdf-7c027680e719\r\n");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7f699909-5287-412c-87fc-244531eaa8c7");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7b9f7fc2-38b5-484f-aaef-ce52333cf198", "3c25edb0-e14e-46ea-a155-401cd22ba74e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b9f7fc2-38b5-484f-aaef-ce52333cf198");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e");
        }
    }
}
