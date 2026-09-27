using KiwiApp.Domain.Entities;
using KiwiApp.Application.UseCases.Bookings.Update;

namespace KiwiApp.Application.Mapping;

public sealed class UpdatePassengerMapper
{
    private readonly Dictionary<object, object> mapped = new(ReferenceEqualityComparer.Instance);

    public Booking ToEntity(UpdateBookingGraphCommand value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (Booking)existing;

        var result = new Booking();
        mapped.Add(value, result);
        result.BookingId = value.BookingId;
        result.FlightId = value.FlightId;
        result.Flight = ToEntity(value.Flight);
        result.Passengers = value.Passengers?.Select(ToEntity).ToList()!;
        result.Status = value.Status;
        result.StripeCheckoutSessionId = value.StripeCheckoutSessionId;
        result.Price = value.Price;
        result.Email = value.Email;
        result.UserId = value.UserId;
        return result;
    }

    public Passenger ToEntity(UpdatePassengerCommand value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (Passenger)existing;

        var result = new Passenger();
        mapped.Add(value, result);
        result.PassengerId = value.PassengerId;
        result.FirstName = value.FirstName;
        result.LastName = value.LastName;
        result.SeatId = value.SeatId;
        result.Seat = ToEntity(value.Seat);
        return result;
    }

    public Flight ToEntity(UpdateFlightGraphCommand value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (Flight)existing;

        var result = new Flight();
        mapped.Add(value, result);
        result.FlightId = value.FlightId;
        result.From = value.From;
        result.To = value.To;
        result.Bookings = value.Bookings?.Select(ToEntity).ToList()!;
        result.FromIata = value.FromIata;
        result.ToIata = value.ToIata;
        result.Airline = value.Airline;
        result.FlightNumber = value.FlightNumber;
        result.DepartureTime = value.DepartureTime;
        result.ArrivalTime = value.ArrivalTime;
        result.Status = value.Status;
        result.Price = value.Price;
        result.Seats = value.Seats?.Select(ToEntity).ToList()!;
        return result;
    }

    public Seat ToEntity(UpdateSeatGraphCommand? value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (Seat)existing;

        var result = new Seat();
        mapped.Add(value, result);
        result.Id = value.Id;
        result.FlightId = value.FlightId;
        result.SeatNumber = value.SeatNumber;
        result.IsOccupied = value.IsOccupied;
        result.Flight = ToEntity(value.Flight);
        return result;
    }

}
