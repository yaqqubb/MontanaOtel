using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otel.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentNewfeaturesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "TotalGuestCount",
                table: "Appointments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Appointments",
                newName: "RoomTypeId");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "CheckOutDate");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                newName: "IX_Appointments_RoomTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AdultCount",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ChildrenCount",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_RoomTypes_RoomTypeId",
                table: "Appointments",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_RoomTypes_RoomTypeId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AdultCount",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CheckInDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ChildrenCount",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "Appointments",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Appointments",
                newName: "TotalGuestCount");

            migrationBuilder.RenameColumn(
                name: "CheckOutDate",
                table: "Appointments",
                newName: "AppointmentDate");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RoomTypeId",
                table: "Appointments",
                newName: "IX_Appointments_RoomId");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "SpecialOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
