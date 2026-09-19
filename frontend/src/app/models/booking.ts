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
    id: string;
    name: string;
    lastname: string;
}