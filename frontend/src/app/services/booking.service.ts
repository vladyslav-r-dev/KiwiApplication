import { API_URL } from '../config/api.config';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  AdminBooking,
  Booking,
  CreateBookingRequest,
  CreateBookingResponse,
} from '../models/booking';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  constructor(private httpClient: HttpClient) {}

  getMyBookings(): Observable<Booking[]> {
    return this.httpClient.get<Booking[]>(`${API_URL}/bookings`);
  }

  createBooking(request: CreateBookingRequest): Observable<CreateBookingResponse> {
    return this.httpClient.post<CreateBookingResponse>(`${API_URL}/bookings`, request);
  }

  getAllBookings() {
    return this.httpClient.get<AdminBooking[]>(`${API_URL}/admin/bookings`);
  }

  deleteBooking(id: number) {
    return this.httpClient.delete(`${API_URL}/bookings/${id}`);
  }
}
