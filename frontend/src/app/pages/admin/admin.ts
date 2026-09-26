import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import {
  AuthService,
  AdminUser,
  AdminBooking
} from '../../services/auth';

@Component({
  selector: 'app-admin',
  imports: [RouterLink],
  templateUrl: './admin.html',
  styleUrl: './admin.scss',
})
export class Admin {
  adminUsers = signal<AdminUser[]>([]);
  adminBookings = signal<AdminBooking[]>([]);

  constructor(private authService: AuthService) {}

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
      }
    });
  }

  loadAdminBookings() {
    this.authService.getAllBookings().subscribe({
      next: (bookings) => {
        this.adminBookings.set(bookings);
      },
      error: (err) => {
        console.error('Failed to load bookings', err);
      }
    });
  }

  deleteBooking(id: number) {
    this.authService.deleteBooking(id).subscribe({
      next: () => {
        this.loadAdminBookings();
      },
      error: (err) => {
        console.error('Failed to delete booking', err);
      }
    });
  }
}