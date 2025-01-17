using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deletedUsersFromMeditation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeditationUser_Users_LikedUsersId",
                table: "MeditationUser");

            migrationBuilder.RenameColumn(
                name: "LikedUsersId",
                table: "MeditationUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MeditationUser_LikedUsersId",
                table: "MeditationUser",
                newName: "IX_MeditationUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeditationUser_Users_UserId",
                table: "MeditationUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeditationUser_Users_UserId",
                table: "MeditationUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MeditationUser",
                newName: "LikedUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_MeditationUser_UserId",
                table: "MeditationUser",
                newName: "IX_MeditationUser_LikedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeditationUser_Users_LikedUsersId",
                table: "MeditationUser",
                column: "LikedUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
