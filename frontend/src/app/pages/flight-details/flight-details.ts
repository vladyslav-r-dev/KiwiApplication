import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FlightsService } from "../../services/flightsService";
import { Flight } from '../../models/flight';
import { Weather } from '../../models/search-flight-result';

@Component({
  selector: 'app-flight-details',
  imports: [],
  templateUrl: './flight-details.html',
  styleUrl: './flight-details.scss',
})

export class FlightDetails implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  public flightId: string | null = null;
  private flightsService = inject(FlightsService);
  flight = signal<Flight | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);
  weatherFrom = signal<Weather | null>(null);
  weatherTo = signal<Weather | null>(null);

  ngOnInit() {
    this.getFlightDetails();
  }

  getFlightDetails() {
    const flightId = this.activatedRoute.snapshot.paramMap.get('id');

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
}
