/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LibraryCardService } from './library-card.service';

describe('Service: LibraryCard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LibraryCardService],
    });
  });

  it('should ...', inject(
    [LibraryCardService],
    (service: LibraryCardService) => {
      expect(service).toBeTruthy();
    }
  ));
});
