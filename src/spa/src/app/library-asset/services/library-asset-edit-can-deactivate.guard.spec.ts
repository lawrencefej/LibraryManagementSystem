import { TestBed } from '@angular/core/testing';

import { LibraryAssetEditCanDeactivateGuard } from './library-asset-edit-can-deactivate.guard';

describe('LibraryAssetEditCanDeactivateGuard', () => {
  let guard: LibraryAssetEditCanDeactivateGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(LibraryAssetEditCanDeactivateGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
