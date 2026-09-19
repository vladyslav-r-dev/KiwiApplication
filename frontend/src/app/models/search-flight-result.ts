import { Flight } from "./flight";

export interface Weather {
  city: string;
  temp: number;
  description: string;
}

export interface SearchFlightResult {
  flight: Flight[];
  weatherFrom: Weather | null;
  weatherTo: Weather | null;
}
