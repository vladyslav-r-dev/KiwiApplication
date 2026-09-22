using KiwiApp.Application.Interfaces;
using KiwiApp.Application.Services;

namespace KiwiApp.Application.Jobs;

public class BookingEmailJob(
    IEmailService emailService,
    PdfService pdfService,
    IBookingRepository bookingRepository)
{
    public async Task SendBookingConfirmationAsync(int bookingId)
    {
        var booking = await bookingRepository.GetByIdWithPassengers(bookingId);

        if (booking is null || booking.Passengers.Count == 0)
        {
            return;
        }

        var firstPassenger = booking.Passengers.First();

        var clientName =
            $"{firstPassenger.FirstName} {firstPassenger.LastName}";

        var pdfBytes = pdfService.GenerateBookingPdf(
            booking.Passengers,
            booking.Email,
            booking.FlightId
        );

        await emailService.SendBookingConfirmationAsync(
            booking.Email,
            clientName,
            booking.FlightId,
            pdfBytes
        );
    }
}