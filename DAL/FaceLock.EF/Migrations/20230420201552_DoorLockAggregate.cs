using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceLock.EF.Migrations
{
    /// <inheritdoc />
    public partial class DoorLockAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 647, DateTimeKind.Local).AddTicks(8038),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 1, 17, 41, 16, 696, DateTimeKind.Local).AddTicks(9446));

            migrationBuilder.CreateTable(
                name: "DoorLocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 256, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorLocks", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "DoorLockHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorLockId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    OpenedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 644, DateTimeKind.Local).AddTicks(1472)),
                    DoorLockId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorLockHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorLockHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorLockHistories_DoorLocks_DoorLockId",
                        column: x => x.DoorLockId,
                        principalTable: "DoorLocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorLockHistories_DoorLocks_DoorLockId1",
                        column: x => x.DoorLockId1,
                        principalTable: "DoorLocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDoorLockAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    DoorLockId = table.Column<int>(type: "int", nullable: false),
                    HasAccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDoorLockAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDoorLockAccesses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDoorLockAccesses_DoorLocks_DoorLockId",
                        column: x => x.DoorLockId,
                        principalTable: "DoorLocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockAccessTokens_DoorLockId",
                table: "DoorLockAccessTokens",
                column: "DoorLockId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockHistories_DoorLockId",
                table: "DoorLockHistories",
                column: "DoorLockId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockHistories_DoorLockId1",
                table: "DoorLockHistories",
                column: "DoorLockId1");

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockHistories_OpenedDateTime",
                table: "DoorLockHistories",
                column: "OpenedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_DoorLockHistories_UserId",
                table: "DoorLockHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDoorLockAccesses_DoorLockId",
                table: "UserDoorLockAccesses",
                column: "DoorLockId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDoorLockAccesses_UserId",
                table: "UserDoorLockAccesses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorLockAccessTokens");

            migrationBuilder.DropTable(
                name: "DoorLockHistories");

            migrationBuilder.DropTable(
                name: "UserDoorLockAccesses");

            migrationBuilder.DropTable(
                name: "DoorLocks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 1, 17, 41, 16, 696, DateTimeKind.Local).AddTicks(9446),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 647, DateTimeKind.Local).AddTicks(8038));
        }
    }
}
