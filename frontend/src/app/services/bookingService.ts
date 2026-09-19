import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Booking, CreateBookingRequest, CreateBookingResponse } from '../models/booking';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  constructor(private httpClient: HttpClient) {}

  getMyBookings(): Observable<Booking[]> {
    return this.httpClient.get<Booking[]>(
      'http://localhost:5086/bookings'
    );
  }

  createBooking(request: CreateBookingRequest): Observable<CreateBookingResponse> {
    return this.httpClient.post<CreateBookingResponse>(
      'http://localhost:5086/bookings',
      request
    );
  }
}

