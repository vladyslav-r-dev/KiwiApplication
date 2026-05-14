namespace KiwiApp.Domain.Entities;

public static class Permissions
{
    public const string FlightCreate = "Flight.Create";
    public const string FlightEdit = "Flight.Edit";
    public const string FlightDelete = "Flight.Delete";

    public const string BookingCreate = "Booking.Create";
    public const string BookingViewOwn = "Booking.ViewOwn";
    public const string BookingViewAll = "Booking.ViewAll";
}