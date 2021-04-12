import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';

import { BasketService } from 'src/app/_services/basket.service';
import { Checkout } from 'src/app/_models/checkout';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { NotificationService } from 'src/app/_services/notification.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css'],
})
export class BasketComponent implements OnInit {
  public basketItems$: Observable<Checkout[]> = of([]);
  public basketItems: Checkout[] = [];

  constructor(
    private basketService: BasketService,
    private checkoutService: CheckoutService,
    private notify: NotificationService
  ) {
    this.basketItems$ = this.basketService.getItemsInBasket();
    this.basketItems$.subscribe((_) => (this.basketItems = _));
  }

  ngOnInit() {}

  removeFromBasket(id: number) {
    this.basketService.removeFromBasket(id);
  }

  clearBasket() {
    this.basketService.clearBasket();
  }

  checkoutAssets(checkouts: Checkout[]) {
    this.checkoutService.checkoutAssets(checkouts).subscribe(
      () => {
        this.notify.success('checked out successfully');
        this.clearBasket();
      },
      (error) => {
        this.notify.error(error);
      }
    );
  }
}
