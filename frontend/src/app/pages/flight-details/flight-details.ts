import { Component, inject, OnInit, signal } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FlightsService } from "../../services/flightsService";
import { BookingService } from "../../services/bookingService";
import { Flight } from '../../models/flight';
import { Weather } from '../../models/search-flight-result';

@Component({
  selector: 'app-flight-details',
  imports: [ReactiveFormsModule],
  templateUrl: './flight-details.html',
  styleUrl: './flight-details.scss',
})

export class FlightDetails implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  public flightId: string | null = null;
  private flightsService = inject(FlightsService);
  private bookingService = inject(BookingService);
  flight = signal<Flight | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);
  weatherFrom = signal<Weather | null>(null);
  weatherTo = signal<Weather | null>(null);
  bookingLoading = signal(false);
  bookingError = signal<string | null>(null);
  bookingForm = new FormGroup({
    email: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    passengers: new FormArray([new FormGroup({
      firstName: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      lastName: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    })], Validators.required),
    selectedSeatNumber: new FormControl<string | null>(null),
  });

  ngOnInit() {
    this.getFlightDetails();
  }

  submitBooking(){
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
      next: response => {
        this.bookingLoading.set(false);

        window.location.href = response.checkoutUrl;
        this.bookingError.set(null);
        this.bookingLoading.set(false);
      },
      error: err => {
        console.error('Failed to create booking:', err);
        this.bookingError.set('Failed to create booking.');
        this.bookingLoading.set(false);
      }
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
    next: data => {
    this.flight.set(data);
    this.weatherFrom.set(data.weatherFrom);
    this.weatherTo.set(data.weatherTo);
    this.loading.set(false);
  },
  error: err => {
    console.error(err);
    this.error.set('Failed to load flight details.');
    this.loading.set(false);
  }
  });
  }

  addPassenger() {
    const passengers = this.bookingForm.controls.passengers as FormArray;
    passengers.push(new FormGroup({
      firstName: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      lastName: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      selectedSeatNumber: new FormControl<string | null>(null),
    }));
  }

  removePassenger(index: number) {
    const passengers = this.bookingForm.controls.passengers as FormArray;
    if (passengers.length > 1) {
      passengers.removeAt(index);
    }
  }
}
