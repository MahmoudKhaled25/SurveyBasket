using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019cb26c-19dc-72af-b5da-a6bf9a8ee67b", "019cb26d-85f0-7755-a93f-bfed39e6a0c5", false, false, "Admin", "ADMIN" },
                    { "019cb26c-19dc-72af-b5da-a6c02c75159d", "019cb26d-85f0-7755-a93f-bfeed55af204", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019cb256-46bf-7fc9-8140-76f6f6aba2b8", 0, "019cb256-46bf-7fc9-8140-76f77a19bb9e", "admin@survey-basket.com", true, "Survey Basket", "Admin", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEE3c1Lcrndx0Vtl7vzYD8quX68rkRBTNT0iXnus0y+wWKfZ8qtEZoUXimG1s9VqLIA==", null, false, "CE48995126B941D2B07095100E9169D8", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permission", "polls:read", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 2, "permission", "polls:add", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 3, "permission", "polls:update", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 4, "permission", "polls:delete", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 5, "permission", "questions:read", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 6, "permission", "questions:add", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 7, "permission", "questions:delete", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 8, "permission", "users:read", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 9, "permission", "users:add", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 10, "permission", "users:update", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 11, "permission", "role:read", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 12, "permission", "role:add", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 13, "permission", "role:update", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" },
                    { 14, "permission", "result:read", "019cb26c-19dc-72af-b5da-a6bf9a8ee67b" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019cb26c-19dc-72af-b5da-a6bf9a8ee67b", "019cb256-46bf-7fc9-8140-76f6f6aba2b8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019cb26c-19dc-72af-b5da-a6c02c75159d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019cb26c-19dc-72af-b5da-a6bf9a8ee67b", "019cb256-46bf-7fc9-8140-76f6f6aba2b8" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019cb26c-19dc-72af-b5da-a6bf9a8ee67b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019cb256-46bf-7fc9-8140-76f6f6aba2b8");
        }
    }
}
