using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedUserMeditationTablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeditationUser");

            migrationBuilder.CreateTable(
                name: "UserLikedMeditations",
                columns: table => new
                {
                    MeditationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikedMeditations", x => new { x.MeditationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserLikedMeditations_Meditations_MeditationId",
                        column: x => x.MeditationId,
                        principalTable: "Meditations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikedMeditations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedMeditations_UserId",
                table: "UserLikedMeditations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikedMeditations");

            migrationBuilder.CreateTable(
                name: "MeditationUser",
                columns: table => new
                {
                    LikedMeditationsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeditationUser", x => new { x.LikedMeditationsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MeditationUser_Meditations_LikedMeditationsId",
                        column: x => x.LikedMeditationsId,
                        principalTable: "Meditations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeditationUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeditationUser_UserId",
                table: "MeditationUser",
                column: "UserId");
        }
    }
}
