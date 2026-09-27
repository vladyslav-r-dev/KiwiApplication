import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { BookingService } from '../../services/booking.service';
import { AdminUser } from '../../models/user';
import { AdminBooking } from '../../models/booking';

@Component({
  selector: 'app-admin',
  imports: [RouterLink],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',
})
export class Admin {
  adminUsers = signal<AdminUser[]>([]);
  adminBookings = signal<AdminBooking[]>([]);

  constructor(
    private authService: AuthService,
    private bookingService: BookingService,
  ) {}

  ngOnInit() {
    this.loadAdminUsers();
    this.loadAdminBookings();
  }

  loadAdminUsers() {
    this.authService.getAllUsers().subscribe({
      next: (users) => {
        this.adminUsers.set(users);
      },
      error: (err) => {
        console.error('Failed to load users', err);
      },
    });
  }

  loadAdminBookings() {
    this.bookingService.getAllBookings().subscribe({
      next: (bookings) => {
        this.adminBookings.set(bookings);
      },
      error: (err) => {
        console.error('Failed to load bookings', err);
      },
    });
  }

  deleteBooking(id: number) {
    this.bookingService.deleteBooking(id).subscribe({
      next: () => {
        this.loadAdminBookings();
      },
      error: (err) => {
        console.error('Failed to delete booking', err);
      },
    });
  }
}
