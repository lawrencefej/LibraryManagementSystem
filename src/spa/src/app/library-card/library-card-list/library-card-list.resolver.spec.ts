import { TestBed } from '@angular/core/testing';

import { LibraryCardListResolver } from './library-card-list.resolver';

describe('LibraryCardListResolver', () => {
  let resolver: LibraryCardListResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(LibraryCardListResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
