import { Injectable, OnDestroy } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BasketService } from 'src/app/basket/services/basket.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryCardDetailComponent } from '../library-card-detail/library-card-detail.component';

@Injectable()
export class LibraryCardCheckoutCanDeactivateGuardService
  implements CanDeactivate<LibraryCardDetailComponent>, OnDestroy
{
  private readonly unsubscribe = new Subject<void>();
  private isBasketActive = false;
  constructor(basketService: BasketService, private readonly notify: NotificationService) {
    basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.isBasketActive = basket.active));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  canDeactivate(component: LibraryCardDetailComponent): boolean | Observable<boolean> {
    if (this.isBasketActive) {
      this.notify.error('Please discard the basket before navigating away');
      return false;
    } else {
      return true;
    }
  }
}
