using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class restore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d9955ac1-3d3c-4f73-a195-638841860658", "AQAAAAIAAYagAAAAEGLOjxi1Y2Psvx9q7dN7/Vhc/lYuIrI/gyoAJinE4hrsL1zwnWGt5852KRWzm0GXqw==", "d859d66e-c57e-4288-8f94-aa16138a4609" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0e96083-f1fc-4833-b899-bfed88ad1ba4", "AQAAAAIAAYagAAAAEPvq+IVWUCjJj+WTuweJeA4QGnXdYuCFJVeHLlr5M5/UJ0OLuGVdLicPXBFBQk7Fwg==", "71dfa8a7-5c78-480c-9c9c-6118fc149e6b" });
        }
    }
}
