export interface Booking {
bookingId: number;
flightId: number;
status: Status;
price: number;
email: string | null;
passengers: Passenger[];
userId: number;
}

export enum Status {
    Pending,
    Confirmed,
    Cancelled,
    Updated,
    Paid
}

export interface Passenger{
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
  status: Status;
  price: number;
  checkoutUrl: string;
}