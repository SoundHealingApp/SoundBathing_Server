using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedLikedMeditations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeditationUser",
                columns: table => new
                {
                    LikedMeditationsId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedUsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeditationUser", x => new { x.LikedMeditationsId, x.LikedUsersId });
                    table.ForeignKey(
                        name: "FK_MeditationUser_Meditations_LikedMeditationsId",
                        column: x => x.LikedMeditationsId,
                        principalTable: "Meditations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeditationUser_Users_LikedUsersId",
                        column: x => x.LikedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeditationUser_LikedUsersId",
                table: "MeditationUser",
                column: "LikedUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeditationUser");
        }
    }
}
