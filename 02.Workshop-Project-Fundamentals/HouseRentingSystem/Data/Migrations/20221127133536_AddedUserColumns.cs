using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class AddedUserColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "06b742a6-1ad8-4cf8-8ea4-81932b35be18", "AQAAAAEAACcQAAAAELAM9i57cfBb//d/pz3ChRagbEkkN5dXjRV5enfIA8clHzSms1MkP3QEs9AnLfij+w==", "a9275e4b-aa60-4756-ba20-bedccc25ce4e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "922bec6e-ba74-4ade-a795-475fe8612d4c", "AQAAAAEAACcQAAAAEIXRdUOQzvN8wABro9h8Y+AkddOTZbGjyeQIapfb6yE2JZX2M7/9rWNy17K0uOQOJw==", "08aaf37e-6800-4349-9ade-98528172a995" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e297f92-6320-4e24-9b79-c2e2802ce322", "AQAAAAEAACcQAAAAEM91AGRqoyaDM/0b/05bLe7Q5YVAegLejaDSV2nRlzB9ubWb4Lc/5JSgR5oa+9bAZA==", "34a4ba4d-713c-4ae8-8445-d9fb06ec2892" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e4f955e-cd03-4812-807b-b3791f541247", "AQAAAAEAACcQAAAAEGkOo9bgvZwki0ESsSzk5pF8EsgGr+af3fMkH4EtIUpdJoP2yZ5uYMVCGQyNmAEp8A==", "f4ffeea4-b5eb-44e1-b9bb-e1e21591b8cd" });
        }
    }
}
