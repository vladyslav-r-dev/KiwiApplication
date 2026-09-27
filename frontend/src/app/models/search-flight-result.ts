import { Weather } from './weather';
import { Flight } from './flight';

export interface SearchFlightResult {
  flight: Flight[];
  weatherFrom: Weather | null;
  weatherTo: Weather | null;
}
