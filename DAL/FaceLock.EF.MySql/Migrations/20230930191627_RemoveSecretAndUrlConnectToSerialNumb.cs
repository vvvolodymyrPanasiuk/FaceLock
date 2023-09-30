using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceLock.EF.MySql.Migrations
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
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 30, 22, 16, 27, 621, DateTimeKind.Local).AddTicks(9135),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 19, 17, 53, 13, 161, DateTimeKind.Local).AddTicks(618));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 30, 22, 16, 27, 611, DateTimeKind.Local).AddTicks(2965),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 8, 19, 17, 53, 13, 157, DateTimeKind.Local).AddTicks(4013));
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
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 19, 17, 53, 13, 161, DateTimeKind.Local).AddTicks(618),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 9, 30, 22, 16, 27, 621, DateTimeKind.Local).AddTicks(9135));

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "DoorLockSecurityInformations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 19, 17, 53, 13, 157, DateTimeKind.Local).AddTicks(4013),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValue: new DateTime(2023, 9, 30, 22, 16, 27, 611, DateTimeKind.Local).AddTicks(2965));
        }
    }
}
