import { TestBed } from '@angular/core/testing';

import { LibraryAssetService } from './library-asset.service';

describe('LibraryAssetService', () => {
  let service: LibraryAssetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LibraryAssetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
