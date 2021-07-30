import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { StateService } from 'src/app/_services/state.service';
import { ILibraryCardDetailResponse } from '../Models/library-card-detail-response';
import { LibraryCardService } from '../services/library-card.service';

@Injectable()
export class LibraryCardDetailResolver implements Resolve<ILibraryCardDetailResponse> {
  constructor(private readonly cardService: LibraryCardService, private readonly stateService: StateService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<ILibraryCardDetailResponse> {
    return forkJoin({
      card: this.cardService.getCardById(route.params.id),
      states: this.stateService.getStatesObject()
    });
  }
}
