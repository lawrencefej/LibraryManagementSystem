import { TestBed } from '@angular/core/testing';

import { LibraryAssetDetailResolver } from './library-asset-detail.resolver';

describe('LibraryAssetDetailResolver', () => {
  let resolver: LibraryAssetDetailResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(LibraryAssetDetailResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
