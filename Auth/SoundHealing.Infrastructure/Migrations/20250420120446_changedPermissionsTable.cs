using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedPermissionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("04cf09e1-5507-9cb7-a6bd-5cae7c538dc1"), "LiveStreamsAdministration" },
                    { new Guid("0b322a17-8807-f1c9-d491-5b8cc2ddec75"), "AddFeedback" },
                    { new Guid("483761cb-6f9d-31b4-e6fe-2e93c3c092fc"), "GetFeedbackInfo" },
                    { new Guid("61010f34-0466-8fb0-49e4-c6bf08cfb7a5"), "QuotesAdministration" },
                    { new Guid("6d0ca4ab-0ac6-b2ab-b9c6-56923682ba6d"), "EditUserInfo" },
                    { new Guid("7d7dbf23-67ab-ec4b-64ac-88f1077d3156"), "GetQuotesInfo" },
                    { new Guid("9c5f49d3-1628-36f6-ea48-d3a582744254"), "ManageMeditationsLikes" },
                    { new Guid("a682ec01-91b3-08cf-6d07-b2ddfea6ecdc"), "GetUserInfo" },
                    { new Guid("b9cb9748-fa53-ebb3-28a9-323b81d62f66"), "GetLiveStreamsInfo" },
                    { new Guid("c6dccaef-07fd-6ae1-aad9-2794b34df2e4"), "MeditationsAdministration" },
                    { new Guid("c9b2967f-0a58-1903-8704-3abf5b64c0eb"), "ManageMeditationsRecommendations" },
                    { new Guid("f2885585-8b0e-0f44-b5ff-0e70f52b8279"), "GetMeditationsInfo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("04cf09e1-5507-9cb7-a6bd-5cae7c538dc1"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("0b322a17-8807-f1c9-d491-5b8cc2ddec75"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("483761cb-6f9d-31b4-e6fe-2e93c3c092fc"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("61010f34-0466-8fb0-49e4-c6bf08cfb7a5"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("6d0ca4ab-0ac6-b2ab-b9c6-56923682ba6d"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("7d7dbf23-67ab-ec4b-64ac-88f1077d3156"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("9c5f49d3-1628-36f6-ea48-d3a582744254"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("a682ec01-91b3-08cf-6d07-b2ddfea6ecdc"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("b9cb9748-fa53-ebb3-28a9-323b81d62f66"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("c6dccaef-07fd-6ae1-aad9-2794b34df2e4"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("c9b2967f-0a58-1903-8704-3abf5b64c0eb"));

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: new Guid("f2885585-8b0e-0f44-b5ff-0e70f52b8279"));
        }
    }
}
