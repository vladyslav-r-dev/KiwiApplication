import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

import { FlightsService } from '../../services/flights.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';

import { Flight } from '../../models/flight';

@Component({
  selector: 'app-flight-details',
  imports: [ReactiveFormsModule, RouterLink, DatePipe],
  templateUrl: './flight-details.html',
  styleUrl: './flight-details.scss',
})
export class FlightDetails implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private flightsService = inject(FlightsService);
  private bookingService = inject(BookingService);

  authService = inject(AuthService);

  public flightId: string | null = null;

  flight = signal<Flight | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  bookingLoading = signal(false);
  bookingError = signal<string | null>(null);

  bookingForm = new FormGroup({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required],
    }),

    passengers: new FormArray(
      [
        new FormGroup({
          firstName: new FormControl('', {
            nonNullable: true,
            validators: [Validators.required],
          }),

          lastName: new FormControl('', {
            nonNullable: true,
            validators: [Validators.required],
          }),

          selectedSeatNumber: new FormControl<string | null>(null),
        }),
      ],
      Validators.required,
    ),
  });

  ngOnInit() {
    this.getFlightDetails();
  }

  submitBooking() {
    if (this.bookingForm.invalid) {
      return;
    }

    const createBookingRequest = {
      flightId: Number(this.flightId),
      email: this.bookingForm.controls.email.value,
      passengers: this.bookingForm.controls.passengers.getRawValue(),
    };

    this.bookingError.set(null);
    this.bookingLoading.set(true);

    this.bookingService.createBooking(createBookingRequest).subscribe({
      next: (response) => {
        this.bookingLoading.set(false);
        window.location.href = response.checkoutUrl;
      },

      error: (err) => {
        console.error('Failed to create booking:', err);
        this.bookingError.set('Failed to create booking.');
        this.bookingLoading.set(false);
      },
    });
  }

  getFlightDetails() {
    const flightId = this.activatedRoute.snapshot.paramMap.get('id');

    this.flightId = flightId;

    if (!flightId) {
      this.loading.set(false);
      this.error.set('Flight ID not found.');
      return;
    }

    this.error.set(null);
    this.loading.set(true);

    this.flightsService.getFlightById(flightId).subscribe({
      next: (data) => {
        console.log('FLIGHT DETAILS:', data);
        this.flight.set(data);
        this.loading.set(false);
      },

      error: (err) => {
        console.error(err);
        this.error.set('Failed to load flight details.');
        this.loading.set(false);
      },
    });
  }

  seatRows = computed(() => {
    const seats = this.flight()?.seats ?? [];

    const rows = new Map<number, typeof seats>();

    for (const seat of seats) {
      const rowNumber = parseInt(seat.seatNumber, 10);

      if (!rows.has(rowNumber)) {
        rows.set(rowNumber, []);
      }

      rows.get(rowNumber)!.push(seat);
    }

    return [...rows.entries()]
      .sort(([a], [b]) => a - b)
      .map(([row, rowSeats]) => ({
        row,

        left: rowSeats.filter((seat) => ['A', 'B', 'C'].includes(seat.seatNumber.slice(-1))),

        right: rowSeats.filter((seat) => ['D', 'E', 'F'].includes(seat.seatNumber.slice(-1))),
      }));
  });

  addPassenger() {
    this.bookingForm.controls.passengers.push(
      new FormGroup({
        firstName: new FormControl('', {
          nonNullable: true,
          validators: [Validators.required],
        }),

        lastName: new FormControl('', {
          nonNullable: true,
          validators: [Validators.required],
        }),

        selectedSeatNumber: new FormControl<string | null>(null),
      }),
    );
  }

  removePassenger(index: number) {
    const passengers = this.bookingForm.controls.passengers;

    if (passengers.length > 1) {
      passengers.removeAt(index);
    }
  }
}
