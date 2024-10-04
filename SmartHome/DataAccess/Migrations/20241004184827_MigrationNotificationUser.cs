using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigrationNotificationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeviceUnitId",
                table: "Notification",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "HomeId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Notification",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DeviceUnitId",
                table: "Notification",
                column: "DeviceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_HomeId",
                table: "Notification",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_DeviceUnits_DeviceUnitId",
                table: "Notification",
                column: "DeviceUnitId",
                principalTable: "DeviceUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Home_HomeId",
                table: "Notification",
                column: "HomeId",
                principalTable: "Home",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_DeviceUnits_DeviceUnitId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Home_HomeId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Users_UserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_DeviceUnitId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_HomeId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "DeviceUnitId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notification");
        }
    }
}
