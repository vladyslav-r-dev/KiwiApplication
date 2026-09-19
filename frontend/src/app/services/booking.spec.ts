import { TestBed } from '@angular/core/testing';

import { Booking } from './booking';

describe('Booking', () => {
  let service: Booking;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Booking);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
