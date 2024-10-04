using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigracionHomeOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Home_OwnerId",
                table: "Home",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Home_Users_OwnerId",
                table: "Home",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Home_Users_OwnerId",
                table: "Home");

            migrationBuilder.DropIndex(
                name: "IX_Home_OwnerId",
                table: "Home");
        }
    }
}
