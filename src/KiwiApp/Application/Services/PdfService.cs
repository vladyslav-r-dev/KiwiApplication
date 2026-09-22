using KiwiApp.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace KiwiApp.Application.Services;

public class PdfService
{
    public byte[] GenerateBookingPdf(
        IEnumerable<Passenger> passengers,
        string clientEmail,
        int flightId
    )
    {
        var passengerList = passengers.ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item()
                        .Text("Booking Confirmation")
                        .FontSize(24)
                        .Bold();

                    column.Item()
                        .Text($"Flight ID: {flightId}");

                    column.Item()
                        .Text($"Email: {clientEmail}");

                    column.Item()
                        .Text("Passengers:")
                        .FontSize(16)
                        .Bold();

                    for (var i = 0; i < passengerList.Count; i++)
                    {
                        var passenger = passengerList[i];

                        column.Item().Text(
                            $"{i + 1}. {passenger.FirstName} {passenger.LastName} - Seat: {passenger.Seat?.SeatNumber}"
                        );
                    }

                    column.Item()
                        .Text("Thank you for choosing KiwiApp.");
                });
            });
        });

        using var memoryStream = new MemoryStream();

        document.GeneratePdf(memoryStream);

        return memoryStream.ToArray();
    }
}