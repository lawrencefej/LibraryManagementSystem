/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LibraryCardCheckoutCanDeactivateGuardService } from './library-card-checkout-can-deactivate-guard.service';

describe('Service: LibraryCardCheckoutCanDeactivateGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LibraryCardCheckoutCanDeactivateGuardService]
    });
  });

  it('should ...', inject([LibraryCardCheckoutCanDeactivateGuardService], (service: LibraryCardCheckoutCanDeactivateGuardService) => {
    expect(service).toBeTruthy();
  }));
});
