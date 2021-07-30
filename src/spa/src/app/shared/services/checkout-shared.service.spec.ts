import { TestBed } from '@angular/core/testing';

import { CheckoutSharedService } from './checkout-shared.service';

describe('CheckoutSharedService', () => {
  let service: CheckoutSharedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CheckoutSharedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
