import { TestBed } from '@angular/core/testing';

import { LibraryAssetListResolver } from './library-asset-list.resolver';

describe('LibraryAssetListResolver', () => {
  let resolver: LibraryAssetListResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(LibraryAssetListResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
