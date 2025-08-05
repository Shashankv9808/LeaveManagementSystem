using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8c133378-9aec-4a33-8d1f-21c83f4b67bc", new DateOnly(1998, 1, 1), "Default", "Administrator", "AQAAAAIAAYagAAAAEPi0CqJzC5k74kiMhQT5B+C7dgZA8R68UlioXv6olHMiTR+j1WkUTdcSYH2ZlcFFlA==", "3bdf0730-df25-4eb7-b312-9d417c6fd5b6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a1958805-7e96-4673-a3df-cc6517ee78a8", "AQAAAAIAAYagAAAAEHXDybl/Y1ksTHKJHc/2GNSMEm7rMzUZWl8oqkUhjl0dnHNssZLXD3o3788GCBARjg==", "82ca149b-e64c-4080-9e53-0c32e04c105f" });
        }
    }
}
