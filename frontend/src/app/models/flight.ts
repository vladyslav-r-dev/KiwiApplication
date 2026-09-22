import { Weather } from "./search-flight-result";
import { Seat } from "./seats";

export interface Flight {
  weatherFrom: Weather | null;
  weatherTo: Weather | null;
  flightId : number;
  from: string;
  to: string;
  bookings: unknown[];
  fromIata: string | null;
  toIata: string | null;
  airline: string | null;
  flightNumber: string | null;
  departureTime: string | null;
  arrivalTime: string | null;
  status: string | null;
  price: number;
  seats: Seat[];
}
