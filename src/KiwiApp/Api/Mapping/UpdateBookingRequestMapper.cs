using KiwiApp.Api.Contracts.Booking;
using KiwiApp.Application.UseCases.Bookings.Update;

namespace KiwiApp.Api.Mapping;

public sealed class UpdateBookingRequestMapper
{
    private readonly Dictionary<object, object> mapped = new(ReferenceEqualityComparer.Instance);

    public UpdateBookingGraphCommand ToCommand(UpdateBookingGraphRequest value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (UpdateBookingGraphCommand)existing;

        var result = new UpdateBookingGraphCommand();
        mapped.Add(value, result);
        result.BookingId = value.BookingId;
        result.FlightId = value.FlightId;
        result.Flight = ToCommand(value.Flight);
        result.Passengers = value.Passengers?.Select(ToCommand).ToList()!;
        result.Status = value.Status;
        result.StripeCheckoutSessionId = value.StripeCheckoutSessionId;
        result.Price = value.Price;
        result.Email = value.Email;
        result.UserId = value.UserId;
        return result;
    }

    public UpdatePassengerCommand ToCommand(UpdatePassengerRequest value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (UpdatePassengerCommand)existing;

        var result = new UpdatePassengerCommand();
        mapped.Add(value, result);
        result.PassengerId = value.PassengerId;
        result.FirstName = value.FirstName;
        result.LastName = value.LastName;
        result.SeatId = value.SeatId;
        result.Seat = ToCommand(value.Seat);
        return result;
    }

    public UpdateFlightGraphCommand ToCommand(UpdateFlightGraphRequest value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (UpdateFlightGraphCommand)existing;

        var result = new UpdateFlightGraphCommand();
        mapped.Add(value, result);
        result.FlightId = value.FlightId;
        result.From = value.From;
        result.To = value.To;
        result.Bookings = value.Bookings?.Select(ToCommand).ToList()!;
        result.FromIata = value.FromIata;
        result.ToIata = value.ToIata;
        result.Airline = value.Airline;
        result.FlightNumber = value.FlightNumber;
        result.DepartureTime = value.DepartureTime;
        result.ArrivalTime = value.ArrivalTime;
        result.Status = value.Status;
        result.Price = value.Price;
        result.Seats = value.Seats?.Select(ToCommand).ToList()!;
        return result;
    }

    public UpdateSeatGraphCommand ToCommand(UpdateSeatGraphRequest? value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (UpdateSeatGraphCommand)existing;

        var result = new UpdateSeatGraphCommand();
        mapped.Add(value, result);
        result.Id = value.Id;
        result.FlightId = value.FlightId;
        result.SeatNumber = value.SeatNumber;
        result.IsOccupied = value.IsOccupied;
        result.Flight = ToCommand(value.Flight);
        return result;
    }

}
