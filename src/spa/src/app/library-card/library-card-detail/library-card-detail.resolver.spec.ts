import { TestBed } from '@angular/core/testing';

import { LibraryCardDetailResolver } from './library-card-detail.resolver';

describe('LibraryCardDetailResolver', () => {
  let resolver: LibraryCardDetailResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(LibraryCardDetailResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
