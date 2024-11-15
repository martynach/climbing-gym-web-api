using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dot_net_api.Migrations
{
    /// <inheritdoc />
    public partial class resourcecreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "ClimbingGyms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClimbingGyms_CreatorId",
                table: "ClimbingGyms",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClimbingGyms_Users_CreatorId",
                table: "ClimbingGyms",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClimbingGyms_Users_CreatorId",
                table: "ClimbingGyms");

            migrationBuilder.DropIndex(
                name: "IX_ClimbingGyms_CreatorId",
                table: "ClimbingGyms");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ClimbingGyms");
        }
    }
}
