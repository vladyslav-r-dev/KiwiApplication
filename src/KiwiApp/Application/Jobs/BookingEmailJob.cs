using KiwiApp.Application.Interfaces;
using KiwiApp.Application.Services;
using KiwiApp.Domain.Entities;

namespace KiwiApp.Application.Jobs;

public class BookingEmailJob(IEmailService  emailService, PdfService pdfService, IBookingRepository  bookingRepository)
{
    public async Task SendBookingConfirmationAsync(int bookingId)
    {
        var booking = await bookingRepository.GetByIdWithPassengers(bookingId);
        
        if (booking is null)
        {
            return;
        }
        
        var firstPassenger = booking.Passengers.First();
        var clientName = firstPassenger.FirstName;

        var pdfBytes = pdfService.GenerateBookingPdf(
            clientName,
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