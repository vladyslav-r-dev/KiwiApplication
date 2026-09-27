using KiwiApp.Domain.Entities;
using KiwiApp.Application.UseCases.Shared;

namespace KiwiApp.Application.Mapping;

public sealed class EntityResultMapper
{
    private readonly Dictionary<object, object> mapped = new(ReferenceEqualityComparer.Instance);

    public BookingResult ToResult(Booking value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (BookingResult)existing;

        var result = new BookingResult();
        mapped.Add(value, result);
        result.BookingId = value.BookingId;
        result.FlightId = value.FlightId;
        result.Flight = ToResult(value.Flight);
        result.Passengers = value.Passengers?.Select(ToResult).ToList()!;
        result.Status = value.Status;
        result.StripeCheckoutSessionId = value.StripeCheckoutSessionId;
        result.Price = value.Price;
        result.Email = value.Email;
        result.UserId = value.UserId;
        return result;
    }

    public PassengerResult ToResult(Passenger value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (PassengerResult)existing;

        var result = new PassengerResult();
        mapped.Add(value, result);
        result.PassengerId = value.PassengerId;
        result.FirstName = value.FirstName;
        result.LastName = value.LastName;
        result.SeatId = value.SeatId;
        result.Seat = ToResult(value.Seat);
        return result;
    }

    public FlightResult ToResult(Flight value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (FlightResult)existing;

        var result = new FlightResult();
        mapped.Add(value, result);
        result.FlightId = value.FlightId;
        result.From = value.From;
        result.To = value.To;
        result.Bookings = value.Bookings?.Select(ToResult).ToList()!;
        result.FromIata = value.FromIata;
        result.ToIata = value.ToIata;
        result.Airline = value.Airline;
        result.FlightNumber = value.FlightNumber;
        result.DepartureTime = value.DepartureTime;
        result.ArrivalTime = value.ArrivalTime;
        result.Status = value.Status;
        result.Price = value.Price;
        result.Seats = value.Seats?.Select(ToResult).ToList()!;
        return result;
    }

    public SeatDetailsResult ToResult(Seat? value)
    {
        if (value is null) return null!;
        if (mapped.TryGetValue(value, out var existing)) return (SeatDetailsResult)existing;

        var result = new SeatDetailsResult();
        mapped.Add(value, result);
        result.Id = value.Id;
        result.FlightId = value.FlightId;
        result.SeatNumber = value.SeatNumber;
        result.IsOccupied = value.IsOccupied;
        result.Flight = ToResult(value.Flight);
        return result;
    }

}
