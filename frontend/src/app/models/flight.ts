import { Weather } from "./search-flight-result";

export interface Flight {
  weatherFrom: Weather | null;
  weatherTo: Weather | null;
  flightId : number;
  from: string;
  to: string;
  bookings: unknown[];
}
