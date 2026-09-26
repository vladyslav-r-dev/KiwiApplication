import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule
} from '@angular/forms';

import { DatePipe } from '@angular/common';

import { FlightsService } from '../../services/flightsService';
import { AuthService } from '../../services/auth';

import { Flight } from '../../models/flight';

import { Login } from '../login/login';
import { Register } from '../register/register';

@Component({
  selector: 'app-home',

  imports: [
    RouterLink,
    ReactiveFormsModule,
    DatePipe,
    Login,
    Register,
  ],

  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {

  fromOptions = signal<string[]>([]);
  toOptions = signal<string[]>([]);

  flights = signal<Flight[]>([]);

  isLoginOpen = false;
  isRegisterOpen = false;

  cardImages: string[] = [
    'https://images.unsplash.com/photo-1474181487882-5abf3f0ba6c2?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1508804185872-d7badad00f7d?auto=format&fit=crop&w=1000&q=80',
    'https://images.unsplash.com/photo-1537531383496-f4749b8032cf?auto=format&fit=crop&w=1000&q=80'
  ];

  searchForm = new FormGroup({
    from: new FormControl(''),
    to: new FormControl(''),
    departureDate: new FormControl(''),
  });

  constructor(
    private flightsService: FlightsService,
    private router: Router,
    public authService: AuthService
  ) {}

  ngOnInit() {
    this.flightsService.getFlights().subscribe({
      next: (data) => {

        this.fromOptions.set([
          ...new Set(
            data.map(flight => flight.from)
          )
        ]);

        this.toOptions.set([
          ...new Set(
            data.map(flight => flight.to)
          )
        ]);

        this.flights.set(
          data.slice(0, 3)
        );
      },

      error: (err) => {
        console.error(err);
      }
    });
  }

  onSearch() {
    const from =
      this.searchForm.get('from')?.value;

    const to =
      this.searchForm.get('to')?.value;

    const departureDate =
      this.searchForm.get('departureDate')?.value;

    this.flightsService.getFlights(
      from ?? null,
      to ?? null,
      null,
      null,
      null,
      null,
      null,
      departureDate ?? null
    ).subscribe({

      next: (flights) => {

        if (flights.length === 1) {

          this.router.navigate([
            '/flights',
            flights[0].flightId
          ]);

          return;
        }

        this.router.navigate(
          ['/flights'],
          {
            queryParams: {
              from,
              to,
              departureDate
            }
          }
        );
      },

      error: (err) => {
        console.error(err);
      }
    });
  }

  toggleLogin() {
    this.isLoginOpen =
      !this.isLoginOpen;
  }

  toggleRegister() {
    this.isRegisterOpen =
      !this.isRegisterOpen;
  }

  closeLogin() {
    this.isLoginOpen = false;
  }

  closeRegister() {
    this.isRegisterOpen = false;
  }
}