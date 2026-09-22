import { Component, signal } from '@angular/core';
import { FlightsService } from "../../services/flightsService";
import { Flight } from "../../models/flight";
import { ActivatedRouteSnapshot, Router, RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Weather } from '../../models/search-flight-result';

@Component({
  selector: 'app-flights',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './flights.html',
  styleUrl: './flights.scss',
})
export class Flights {
onReset() {
  this.searchForm.reset();
  this.filteredFlights.set(this.flights());
}
  
  flights = signal<Flight[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  fromOptions = signal<string[]>([]);
  toOptions = signal<string[]>([]);
  weatherFrom = signal<Weather | null>(null);
  weatherTo = signal<Weather | null>(null);

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
  
  constructor(private flightsService: FlightsService, private router: Router) {
    
  }
  
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
      this.fromOptions.set([...new Set(data.map(flight => flight.from))]);
      this.toOptions.set([...new Set(data.map(flight => flight.to))]);
    },
    error: (err) => {
      this.error.set('Failed to load flights');
      this.loading.set(false);
    }
  });
}

filteredFlights = signal<Flight[]>([]);

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
    
      this.flightsService.getFlights(
        from ?? null,
        to ?? null,
        status ?? null,
        minPrice ? Number(minPrice) : null,
        maxPrice ? Number(maxPrice) : null,
        sortBy ?? null,
        airline ?? null,
        departureDate ?? null
      ).subscribe({
        next: (data) => {
          this.filteredFlights.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          console.error(err);
          this.error.set('Failed to search flights.');
          this.loading.set(false);
        }
      });
    }
  }
