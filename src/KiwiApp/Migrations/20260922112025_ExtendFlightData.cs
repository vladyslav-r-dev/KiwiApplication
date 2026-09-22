using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KiwiApp.Migrations
{
    /// <inheritdoc />
    public partial class ExtendFlightData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Airline",
                table: "Flights",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalTime",
                table: "Flights",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureTime",
                table: "Flights",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromIata",
                table: "Flights",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Flights",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Flights",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToIata",
                table: "Flights",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Airline",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ArrivalTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "DepartureTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FromIata",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ToIata",
                table: "Flights");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
