using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace KiwiApp.Application.Services;

public class PdfService
{
    public byte[] GenerateBookingPdf(
        string clientName,
        string clientEmail,
        int flightId
    )
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content().Column(column =>
                {
                    column.Item().Text("Booking Confirmation").FontSize(24).Bold();
                    column.Item().Text($"Passenger: {clientName}");
                    column.Item().Text($"Email: {clientEmail}");
                    column.Item().Text($"Flight ID: {flightId}");
                    column.Item().Text("Thank you for choosing KiwiApp.");
                });
            });
        });

        using var memoryStream = new MemoryStream();
        document.GeneratePdf(memoryStream);
        return memoryStream.ToArray();
    }
}