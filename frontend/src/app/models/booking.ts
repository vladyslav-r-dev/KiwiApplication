export interface Booking {
  bookingId: number;
  flightId: number;
  status: BookingStatus;
  price: number;
  email: string | null;
  passengers: Passenger[];
  userId: number;
}

export enum BookingStatus {
  Pending,
  Confirmed,
  Cancelled,
  Updated,
  Paid,
}

export interface Passenger {
  firstName: string;
  lastName: string;
}

export interface CreateBookingRequest {
  flightId: number;
  email: string;
  passengers: Passenger[];
}

export interface CreateBookingResponse {
  bookingId: number;
  status: BookingStatus;
  price: number;
  checkoutUrl: string;
}
export interface AdminBooking {
  bookingId: number;
  userId: number;
  flightId: number;
  email: string;
  status: string;
  price: number;
}
