import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { StateService } from 'src/app/_services/state.service';
import { ILibraryCardDetailResponse } from '../Models/ILibraryCardDetailResponse';
import { LibraryCardService } from '../services/library-card.service';

@Injectable()
export class LibraryCardDetailResolver implements Resolve<ILibraryCardDetailResponse> {
  constructor(
    private readonly cardService: LibraryCardService,
    private readonly checkoutService: CheckoutService,
    private readonly stateService: StateService
  ) {}
  resolve(route: ActivatedRouteSnapshot): Observable<ILibraryCardDetailResponse> {
    return forkJoin({
      card: this.cardService.getCardById(route.params.id),
      checkouts: this.checkoutService.getCheckoutsForCard(route.params.id),
      states: this.stateService.getStatesObject()
    });
  }
}
