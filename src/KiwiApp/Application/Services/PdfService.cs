using IronPdf;

namespace KiwiApp.Application.Services;

public class PdfService
{
    public byte[] GenerateBookingPdf(
        string clientName,
        string clientEmail,
        int flightId
    )
    {
        var html = $$"""

                     <html>
                     <head>
                         <style>
                             body {
                                 font-family: Arial, sans-serif;
                                 padding: 40px;
                             }

                             .card {
                                 border: 1px solid #ddd;
                                 border-radius: 12px;
                                 padding: 24px;
                             }

                             h1 {
                                 color: #2d7a46;
                             }
                         </style>
                     </head>
                     <body>
                         <div class="card">
                             <h1>Booking Confirmation</h1>

                             <p><strong>Passenger:</strong> {{clientName}}</p>
                             <p><strong>Email:</strong> {{clientEmail}}</p>
                             <p><strong>Flight ID:</strong> {{flightId}}</p>

                             <p>Thank you for choosing KiwiApp.</p>
                         </div>
                     </body>
                     </html>
                     """;

        var renderer = new ChromePdfRenderer();

        var pdf = renderer.RenderHtmlAsPdf(html);

        return pdf.BinaryData;
    }
}