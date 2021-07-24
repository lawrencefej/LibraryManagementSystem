import { Component } from '@angular/core';
import { Subject } from 'rxjs';

import { BasketService } from 'src/app/_services/basket.service';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { takeUntil } from 'rxjs/operators';
import { BasketViewModel } from '../models/basket-view-model';

@Component({
  selector: 'lms-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent {
  private readonly unsubscribe = new Subject<void>();
  public basket: BasketViewModel;

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private notify: NotificationService
  ) {
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.basket = basket));
  }

  removeFromBasket(assetId: number) {
    this.basketService.removeFromBasket(assetId);
  }

  clearBasket() {
    this.basketService.clearBasket();
  }

  checkoutBasket() {
    this.checkoutService.checkoutBasket(this.basket).subscribe(
      () => {
        this.notify.success('checked out successfully');
        this.basketService.completeTransaction();
        this.clearBasket();
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
