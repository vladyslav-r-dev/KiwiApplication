using KiwiApp.Application.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace KiwiApp.Application.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendBookingConfirmationAsync(
        string clientEmail,
        string clientName,
        int flightId,
        byte[] pdfBytes)
    {
        var apiKey = configuration["SendGrid:SendGridKey"];

        var client = new SendGridClient(apiKey);

        var from = new EmailAddress("unitydiee@gmail.com", "KiwiApp");
        
        var to = new EmailAddress(clientEmail, clientName);

        var subject = "Your KiwiApp booking confirmation";

        var plainTextContent = $"""
                                Hello, {clientName}!

                                Your booking has been paid successfully.

                                Flight ID: {flightId}

                                Your booking confirmation PDF is attached.

                                Thank you for choosing KiwiApp.
                                """;

        var htmlContent = $"""
                           <p>Hello, <strong>{clientName}</strong>!</p>

                           <p>Your booking has been paid successfully.</p>

                           <p><strong>Flight ID:</strong> {flightId}</p>

                           <p>Your booking confirmation PDF is attached.</p>

                           <p>Thank you for choosing KiwiApp.</p>
                           """;

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            plainTextContent,
            htmlContent
        );

        msg.AddAttachment(
            "booking-confirmation.pdf",
            Convert.ToBase64String(pdfBytes),
            "application/pdf"
        );

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Body.ReadAsStringAsync();
            throw new InvalidOperationException($"SendGrid email failed: {response.StatusCode}. {body}");
        }
    }
}