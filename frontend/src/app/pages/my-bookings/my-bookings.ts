import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking';

@Component({
  selector: 'app-my-bookings',
  imports: [RouterLink],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.scss',
})
export class MyBookings {
  bookingService = inject(BookingService);

  bookings = signal<Booking[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit() {
    this.loading.set(true);

    this.bookingService.getMyBookings().subscribe({
      next: (result) => {
        this.bookings.set(result);
        this.loading.set(false);
      },

      error: (error) => {
        console.error('Loading bookings failed:', error);
        this.error.set('Failed to load bookings');
        this.loading.set(false);
      },
    });
  }
}
