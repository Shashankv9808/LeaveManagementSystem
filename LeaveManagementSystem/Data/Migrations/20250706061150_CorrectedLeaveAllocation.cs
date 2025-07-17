using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedLeaveAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "LeaveAllocations",
                newName: "NumberOfDays");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0bb8ff67-e6fb-4aa8-8c71-e1838ab54829", "AQAAAAIAAYagAAAAEIqOiCQVWIwFaup9c3ZgrtwXTsN3ZI6yrozpxNOlMswsTDnioJCtFf1i7Rmezo+cTg==", "40467957-e43a-41fe-a458-51c93dcc3f3f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfDays",
                table: "LeaveAllocations",
                newName: "MyProperty");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c25edb0-e14e-46ea-a155-401cd22ba74e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3ad5d730-9435-4ee3-bf2d-900b6a4cd498", "AQAAAAIAAYagAAAAEInLqSkgY18JCjHPAUIyfwKtquiWPwKIoYg3ZJtIvhhkEu57AHn90P9/ZAhKuuN3GA==", "62d970d2-2bda-432a-b21d-6237a9627d2a" });
        }
    }
}
