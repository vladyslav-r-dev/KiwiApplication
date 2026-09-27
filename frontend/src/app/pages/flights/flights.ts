import { Component, signal } from '@angular/core';

import { RouterLink } from '@angular/router';

import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

import { DatePipe } from '@angular/common';

import { FlightsService } from '../../services/flights.service';
import { AuthService } from '../../services/auth.service';

import { Flight } from '../../models/flight';

@Component({
  selector: 'app-flights',

  imports: [RouterLink, ReactiveFormsModule, DatePipe],

  templateUrl: './flights.html',
  styleUrl: './flights.scss',
})
export class Flights {
  flights = signal<Flight[]>([]);
  filteredFlights = signal<Flight[]>([]);

  loading = signal(true);
  error = signal<string | null>(null);

  fromOptions = signal<string[]>([]);
  toOptions = signal<string[]>([]);

  searchForm = new FormGroup({
    from: new FormControl(''),
    to: new FormControl(''),
    status: new FormControl(''),
    minPrice: new FormControl<number | null>(null),
    maxPrice: new FormControl<number | null>(null),
    sortBy: new FormControl(''),
    airline: new FormControl(''),
    departureDate: new FormControl(''),
  });

  cardImages: string[] = [
    'https://images.unsplash.com/photo-1474181487882-5abf3f0ba6c2?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1508804185872-d7badad00f7d?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1537531383496-f4749b8032cf?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1514565131-fce0801e5785?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1519501025264-65ba15a82390?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1522083165195-3424ed129620?auto=format&fit=crop&w=1000&q=80',
  ];

  constructor(
    private flightsService: FlightsService,
    public authService: AuthService,
  ) {}

  ngOnInit() {
    this.loadFlights();
  }

  loadFlights() {
    this.error.set(null);
    this.loading.set(true);

    this.flightsService.getFlights().subscribe({
      next: (data) => {
        this.flights.set(data);

        this.filteredFlights.set(data);

        this.loading.set(false);

        this.fromOptions.set([...new Set(data.map((flight) => flight.from))]);

        this.toOptions.set([...new Set(data.map((flight) => flight.to))]);
      },

      error: () => {
        this.error.set('Failed to load flights');

        this.loading.set(false);
      },
    });
  }

  onSearch() {
    this.reactiveSearch();
  }

  reactiveSearch() {
    const from = this.searchForm.get('from')?.value;

    const to = this.searchForm.get('to')?.value;

    const status = this.searchForm.get('status')?.value;

    const minPrice = this.searchForm.get('minPrice')?.value;

    const maxPrice = this.searchForm.get('maxPrice')?.value;

    const sortBy = this.searchForm.get('sortBy')?.value;

    const airline = this.searchForm.get('airline')?.value;

    const departureDate = this.searchForm.get('departureDate')?.value;

    this.error.set(null);
    this.loading.set(true);

    this.flightsService
      .getFlights(
        from ?? null,
        to ?? null,
        status ?? null,

        minPrice ? Number(minPrice) : null,

        maxPrice ? Number(maxPrice) : null,

        sortBy ?? null,
        airline ?? null,
        departureDate ?? null,
      )
      .subscribe({
        next: (data) => {
          this.filteredFlights.set(data);

          this.loading.set(false);
        },

        error: (err) => {
          console.error(err);

          this.filteredFlights.set([]);

          this.error.set('Failed to search flights.');

          this.loading.set(false);
        },
      });
  }

  onReset() {
    this.searchForm.reset();

    this.error.set(null);

    this.filteredFlights.set(this.flights());
  }
}
