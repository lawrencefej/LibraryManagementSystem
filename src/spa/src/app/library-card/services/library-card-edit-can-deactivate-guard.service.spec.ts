/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LibraryCardEditCanDeactivateGuardService } from './library-card-edit-can-deactivate-guard.service';

describe('Service: LibraryCardDetailCanDeactivateGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LibraryCardEditCanDeactivateGuardService]
    });
  });

  it('should ...', inject(
    [LibraryCardEditCanDeactivateGuardService],
    (service: LibraryCardEditCanDeactivateGuardService) => {
      expect(service).toBeTruthy();
    }
  ));
});
