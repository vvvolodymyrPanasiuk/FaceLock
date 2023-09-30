using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceLock.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSecretAndUrlConnectToSerialNumb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "DoorLockSecurityInformations");

            migrationBuilder.RenameColumn(
                name: "UrlConnection",
                table: "DoorLockSecurityInformations",
                newName: "SerialNumber");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 30, 22, 11, 14, 733, DateTimeKind.Local).AddTicks(3783),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 607, DateTimeKind.Local).AddTicks(647));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 30, 22, 11, 14, 728, DateTimeKind.Local).AddTicks(2242),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 601, DateTimeKind.Local).AddTicks(4590));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "DoorLockSecurityInformations",
                newName: "UrlConnection");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 607, DateTimeKind.Local).AddTicks(647),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 9, 30, 22, 11, 14, 733, DateTimeKind.Local).AddTicks(3783));

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "DoorLockSecurityInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 19, 15, 41, 18, 601, DateTimeKind.Local).AddTicks(4590),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 9, 30, 22, 11, 14, 728, DateTimeKind.Local).AddTicks(2242));
        }
    }
}
