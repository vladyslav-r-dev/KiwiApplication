import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Flights } from './flights';

describe('Flights', () => {
  let component: Flights;
  let fixture: ComponentFixture<Flights>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Flights],
    }).compileComponents();

    fixture = TestBed.createComponent(Flights);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
