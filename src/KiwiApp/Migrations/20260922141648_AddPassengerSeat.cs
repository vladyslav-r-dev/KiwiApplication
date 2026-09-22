using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KiwiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPassengerSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Passengers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Seat_SeatId",
                table: "Passengers",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Seat_SeatId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Passengers");
        }
    }
}
