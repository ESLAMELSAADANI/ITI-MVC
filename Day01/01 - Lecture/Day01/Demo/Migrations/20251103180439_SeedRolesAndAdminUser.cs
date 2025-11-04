using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Demo.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6f781404-8f89-4699-93ea-415a714f1d8c", null, "Student", "STUDENT" },
                    { "7add2ebe-4eec-4a32-a0de-701f06774bc9", null, "Instructor", "INSTRUCTOR" },
                    { "d58d3875-d822-4283-8537-21c965172bf9", null, "User", "USER" },
                    { "f69a910e-11da-4b5e-a9bf-c18f189a7c18", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StudentId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a6797cb1-bac8-4c0e-9bf6-e29b6e74dfc2", 0, 23, "STATIC-CONCURRENCY-STAMP", "eslam@gmail.com", false, false, null, "ESLAM@GMAIL.COM", "ESLAM ELSAADANY", "AQAAAAIAAYagAAAAEKHr+lr2tsvTe8ijmGJqPIUpruCb2HRoxXcYEnOvAtgcshi89rBpcwf6n6hx5kxpKQ==", null, false, "STATIC-SECURITY-STAMP", null, false, "Eslam Elsaadany" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f69a910e-11da-4b5e-a9bf-c18f189a7c18", "a6797cb1-bac8-4c0e-9bf6-e29b6e74dfc2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f781404-8f89-4699-93ea-415a714f1d8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7add2ebe-4eec-4a32-a0de-701f06774bc9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d58d3875-d822-4283-8537-21c965172bf9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f69a910e-11da-4b5e-a9bf-c18f189a7c18", "a6797cb1-bac8-4c0e-9bf6-e29b6e74dfc2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f69a910e-11da-4b5e-a9bf-c18f189a7c18");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a6797cb1-bac8-4c0e-9bf6-e29b6e74dfc2");
        }
    }
}
