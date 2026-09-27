import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { FlightDetails } from './flight-details';
import { FlightsService } from '../../services/flights.service';
import { BookingService } from '../../services/booking.service';
import { AuthService } from '../../services/auth.service';
import { Flight } from '../../models/flight';

describe('FlightDetails price', () => {
  it('adds EUR 15 per manually selected seat and updates after changes', () => {
    TestBed.configureTestingModule({ providers: [
      { provide: ActivatedRoute, useValue: {} },
      { provide: FlightsService, useValue: {} },
      { provide: BookingService, useValue: {} },
      { provide: AuthService, useValue: {} },
    ] });
    const component = TestBed.runInInjectionContext(() => new FlightDetails());
    component.flight.set({ price: 100 } as Flight);
    expect(component.totalPrice).toBe(100);
    component.bookingForm.controls.passengers.at(0).controls.selectedSeatNumber.setValue('1A');
    expect(component.totalPrice).toBe(115);
    component.addPassenger();
    expect(component.totalPrice).toBe(215);
    component.bookingForm.controls.passengers.at(1).controls.selectedSeatNumber.setValue('1B');
    expect(component.totalPrice).toBe(230);
    component.bookingForm.controls.passengers.at(0).controls.selectedSeatNumber.setValue(null);
    expect(component.totalPrice).toBe(215);
    component.removePassenger(1);
    expect(component.totalPrice).toBe(100);
  });
});
