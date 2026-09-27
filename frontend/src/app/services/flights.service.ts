import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Flight } from '../models/flight';
import { SearchFlightResult } from '../models/search-flight-result';

@Injectable({
  providedIn: 'root',
})
export class FlightsService {
  constructor(private httpClient: HttpClient) {}

  public getFlights(
    from: string | null = null,
    to: string | null = null,
    status: string | null = null,
    minPrice: number | null = null,
    maxPrice: number | null = null,
    sortBy: string | null = null,
    airline: string | null = null,
    departureDate: string | null = null,
  ): Observable<Flight[]> {
    const params: any = {};
    if (from) {
      params.from = from;
    }
    if (to) {
      params.to = to;
    }
    if (status) {
      params.status = status;
    }
    if (minPrice !== null) {
      params.minPrice = minPrice;
    }
    if (maxPrice !== null) {
      params.maxPrice = maxPrice;
    }
    if (sortBy) {
      params.sortBy = sortBy;
    }
    if (airline) {
      params.airline = airline;
    }
    if (departureDate) {
      params.departureDate = departureDate;
    }

    return this.httpClient.get<Flight[]>('http://localhost:5086/flights', { params });
  }

  public getFlightById(flightId: string): Observable<Flight> {
    return this.httpClient.get<Flight>(`http://localhost:5086/flights/${flightId}`);
  }

  public searchFlights(from: string | null, to: string | null): Observable<SearchFlightResult> {
    const params: any = {};
    if (from) {
      params.from = from;
    }
    if (to) {
      params.to = to;
    }

    return this.httpClient.get<SearchFlightResult>('http://localhost:5086/search/flight', {
      params,
    });
  }
}
