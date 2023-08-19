using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceLock.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdSecurityOfDoorLock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorLockAccessTokens");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 607, DateTimeKind.Local).AddTicks(647),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 25, 23, 36, 16, 424, DateTimeKind.Local).AddTicks(1025));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 601, DateTimeKind.Local).AddTicks(4590),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 25, 23, 36, 16, 420, DateTimeKind.Local).AddTicks(9814));

            migrationBuilder.CreateTable(
                name: "DoorLockSecurityInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorLockId = table.Column<int>(type: "int", nullable: false),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlConnection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoorLockId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorLockSecurityInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorLockSecurityInformations_DoorLocks_DoorLockId",
                        column: x => x.DoorLockId,
                        principalTable: "DoorLocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorLockSecurityInformations_DoorLocks_DoorLockId1",
                        column: x => x.DoorLockId1,
                        principalTable: "DoorLocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockSecurityInformations_DoorLockId",
                table: "DoorLockSecurityInformations",
                column: "DoorLockId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockSecurityInformations_DoorLockId1",
                table: "DoorLockSecurityInformations",
                column: "DoorLockId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorLockSecurityInformations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 25, 23, 36, 16, 424, DateTimeKind.Local).AddTicks(1025),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 607, DateTimeKind.Local).AddTicks(647));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 25, 23, 36, 16, 420, DateTimeKind.Local).AddTicks(9814),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 601, DateTimeKind.Local).AddTicks(4590));

            migrationBuilder.CreateTable(
                name: "DoorLockAccessTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorLockId = table.Column<int>(type: "int", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Utilized = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorLockAccessTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorLockAccessTokens_DoorLocks_DoorLockId",
                        column: x => x.DoorLockId,
                        principalTable: "DoorLocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockAccessTokens_DoorLockId",
                table: "DoorLockAccessTokens",
                column: "DoorLockId");
        }
    }
}
