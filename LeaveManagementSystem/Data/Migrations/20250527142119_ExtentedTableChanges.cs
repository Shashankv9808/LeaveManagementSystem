using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtentedTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "55f01d71-f12e-4608-8d16-fc2edc1007f5", "AQAAAAIAAYagAAAAEP7476N4psuvZcrKsve3trbHxflg3ex7CH9l49ZgGS2VbodxJjAH8SKY1WnI78VjoA==", "4b7e3065-d0e1-470b-8847-1784336aed4b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8c133378-9aec-4a33-8d1f-21c83f4b67bc", "AQAAAAIAAYagAAAAEPi0CqJzC5k74kiMhQT5B+C7dgZA8R68UlioXv6olHMiTR+j1WkUTdcSYH2ZlcFFlA==", "3bdf0730-df25-4eb7-b312-9d417c6fd5b6" });
        }
    }
}
