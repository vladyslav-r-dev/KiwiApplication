using KiwiApp.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace KiwiApp.Application.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendBookingConfirmationAsync(
        string clientEmail,
        string clientName,
        int flightId,
        byte[] pdfBytes)
    {
        var host = configuration["Email:SmtpHost"] ?? "localhost";
        var port = int.TryParse(configuration["Email:SmtpPort"], out var parsedPort)
            ? parsedPort
            : 1025;

        var fromAddress = configuration["Email:From"] ?? "noreply@kiwiapp.local";

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("KiwiApp", fromAddress));
        message.To.Add(new MailboxAddress(clientName, clientEmail));
        message.Subject = "Your KiwiApp booking confirmation";

        var bodyBuilder = new BodyBuilder
        {
            TextBody = $"""
                        Hello, {clientName}!

                        Your booking has been paid successfully.

                        Flight ID: {flightId}

                        Your booking confirmation PDF is attached.

                        Thank you for choosing KiwiApp.
                        """,

            HtmlBody = $"""
                        <p>Hello, <strong>{clientName}</strong>!</p>

                        <p>Your booking has been paid successfully.</p>

                        <p><strong>Flight ID:</strong> {flightId}</p>

                        <p>Your booking confirmation PDF is attached.</p>

                        <p>Thank you for choosing KiwiApp.</p>
                        """
        };

        bodyBuilder.Attachments.Add(
            "booking-confirmation.pdf",
            pdfBytes,
            ContentType.Parse("application/pdf")
        );

        message.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            host,
            port,
            MailKit.Security.SecureSocketOptions.None
        );

        await smtpClient.SendAsync(message);
        await smtpClient.DisconnectAsync(true);
    }
}