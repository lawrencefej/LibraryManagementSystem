import { TestBed } from '@angular/core/testing';

import { CheckoutListResolver } from './checkout-list.resolver';

describe('CheckoutListResolver', () => {
  let resolver: CheckoutListResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(CheckoutListResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
