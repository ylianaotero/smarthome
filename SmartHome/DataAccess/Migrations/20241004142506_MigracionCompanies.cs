using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MigracionCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Company_Users_OwnerId",
                table: "Company");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Company_CompanyId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Company_CompanyId",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Company",
                table: "Company");

            migrationBuilder.RenameTable(
                name: "Company",
                newName: "Companies");

            migrationBuilder.RenameIndex(
                name: "IX_Company_OwnerId",
                table: "Companies",
                newName: "IX_Companies_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Companies_CompanyId",
                table: "Role",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_OwnerId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Companies_CompanyId",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Company");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_OwnerId",
                table: "Company",
                newName: "IX_Company_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Company",
                table: "Company",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Company_Users_OwnerId",
                table: "Company",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Company_CompanyId",
                table: "Devices",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Company_CompanyId",
                table: "Role",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
