using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigracionRoomsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceUnits_Room_RoomId",
                table: "DeviceUnits");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceUnits_Room_RoomId",
                table: "DeviceUnits",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceUnits_Room_RoomId",
                table: "DeviceUnits");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceUnits_Room_RoomId",
                table: "DeviceUnits",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
