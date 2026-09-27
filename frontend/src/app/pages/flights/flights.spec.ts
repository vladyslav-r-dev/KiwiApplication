import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of } from 'rxjs';
import { vi } from 'vitest';
import { Flights } from './flights';
import { FlightsService } from '../../services/flights.service';
import { AuthService } from '../../services/auth.service';

describe('Flights query parameters', () => {
  it('applies the Home filters after loading options, and reset restores the full list', () => {
    const all = [{ flightId: 1, from: 'Oslo', to: 'Paris' }, { flightId: 2, from: 'London', to: 'Rome' }];
    const getFlights = vi.fn().mockReturnValueOnce(of(all)).mockReturnValueOnce(of([all[0]]));
    const component = new Flights(
      { getFlights } as unknown as FlightsService,
      {} as AuthService,
      { snapshot: { queryParamMap: convertToParamMap({ from: 'Oslo', to: 'Paris', departureDate: '2026-10-01' }) } } as ActivatedRoute,
    );
    component.ngOnInit();
    expect(getFlights).toHaveBeenLastCalledWith('Oslo', 'Paris', '', null, null, '', '', '2026-10-01');
    expect(component.filteredFlights()).toEqual([all[0]]);
    expect(component.fromOptions()).toEqual(['Oslo', 'London']);
    component.onReset();
    expect(component.filteredFlights()).toEqual(all);
  });

  it('does not make a second request without Home filters', () => {
    const getFlights = vi.fn().mockReturnValue(of([]));
    const component = new Flights(
      { getFlights } as unknown as FlightsService,
      {} as AuthService,
      { snapshot: { queryParamMap: convertToParamMap({}) } } as ActivatedRoute,
    );
    component.ngOnInit();
    expect(getFlights).toHaveBeenCalledTimes(1);
  });
});
