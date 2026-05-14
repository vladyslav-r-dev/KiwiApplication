using KiwiApp.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace KiwiApp.Application.Services;

public class EmailService  : IEmailService
{
    public async Task SendBookingConfirmationAsync(string clientEmail,
        string clientName,
        int flightId,
        byte[] pdfBytes)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("KiwiApp", "test@kiwiapp.com"));
        message.To.Add(new MailboxAddress(clientName, clientEmail));
        message.Subject = "Test email from KiwiApp";
        
        var bodyBuilder = new BodyBuilder
        {
            TextBody = $"""
                        Hello, {clientName}!

                        Your booking has been created successfully.

                        Flight ID: {flightId}

                        Thank you for choosing KiwiApp.
                        """
        };

        bodyBuilder.Attachments.Add(
            "booking-confirmation.pdf",
            pdfBytes,
            new ContentType("application", "pdf")
        );

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(
            "sandbox.smtp.mailtrap.io",
            587,
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            "4a1c14183d46f5",
            "f5eea3d74d0311"
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}