import { Injectable } from '@angular/core';
import { Router, Resolve, RouterStateSnapshot, ActivatedRouteSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { ILibraryCardDetailResponse } from '../Models/ILibraryCardDetailResponse';
import { LibraryCardService } from '../services/library-card.service';

@Injectable()
export class LibraryCardDetailResolver implements Resolve<ILibraryCardDetailResponse> {
  constructor(
    private readonly cardService: LibraryCardService,
    private readonly checkoutService: CheckoutService,
    private readonly route: Router,
    private readonly notify: NotificationService
  ) {}
  resolve(route: ActivatedRouteSnapshot): Observable<ILibraryCardDetailResponse> {
    return forkJoin({
      card: this.cardService.getCardById(route.params.id),
      checkouts: this.checkoutService.getCheckoutsForCard(route.params.id)
    });
  }
}
