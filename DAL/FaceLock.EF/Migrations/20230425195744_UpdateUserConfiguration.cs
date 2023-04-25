using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaceLock.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 25, 22, 57, 44, 253, DateTimeKind.Local).AddTicks(8296),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 647, DateTimeKind.Local).AddTicks(8038));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 25, 22, 57, 44, 250, DateTimeKind.Local).AddTicks(9066),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 644, DateTimeKind.Local).AddTicks(1472));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                table: "Visits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 647, DateTimeKind.Local).AddTicks(8038),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 25, 22, 57, 44, 253, DateTimeKind.Local).AddTicks(8296));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpenedDateTime",
                table: "DoorLockHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 4, 20, 23, 15, 52, 644, DateTimeKind.Local).AddTicks(1472),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 4, 25, 22, 57, 44, 250, DateTimeKind.Local).AddTicks(9066));
        }
    }
}
