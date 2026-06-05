using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KiwiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeCheckoutSessionIdToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCheckoutSessionId",
                table: "Bookings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCheckoutSessionId",
                table: "Bookings");
        }
    }
}
