import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Flight } from '../models/flight';
import { SearchFlightResult } from '../models/search-flight-result';

@Injectable({
  providedIn: 'root',
})
export class FlightsService {

  constructor(private httpClient: HttpClient) {
  }

  public getFlights(): Observable<Flight[]> {
    return this.httpClient.get<Flight[]>(
      'http://localhost:5086/flights'
    );
  }

  public getFlightById(flightId: string): Observable<Flight> {
    return this.httpClient.get<Flight>(
      `http://localhost:5086/flights/${flightId}`
    );
  }

  public searchFlights(from: string | null, to: string | null): Observable<SearchFlightResult> {
    const params: any = {};
    if (from) {
      params.from = from;
    }
    if (to) {
      params.to = to;
    }

    return this.httpClient.get<SearchFlightResult>(
      'http://localhost:5086/search/flight',
      { params }
    );
  }
}
