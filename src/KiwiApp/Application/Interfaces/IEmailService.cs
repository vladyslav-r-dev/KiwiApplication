namespace KiwiApp.Application.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(
        string clientEmail,
        string clientName,
        int flightId,byte[] pdfBytes
    );
}