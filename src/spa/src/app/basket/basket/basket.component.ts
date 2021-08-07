import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { BasketViewModel } from '../models/basket-view-model';
import { BasketService } from '../services/basket.service';

@Component({
  selector: 'lms-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  basket?: BasketViewModel;
  serverValidationErrors: string[] = [];

  constructor(private basketService: BasketService, private notify: NotificationService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.basket = basket));
  }

  removeFromBasket(assetId: number): void {
    this.basketService.removeFromBasket(assetId);
  }

  clearBasket(): void {
    this.basketService.clearBasket();
  }

  checkoutBasket(): void {
    this.basketService
      .checkoutBasket(this.basket)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        () => {
          this.notify.success('items checked out successfully');
          this.basketService.completeTransaction();
          this.clearBasket();
        },
        error => (this.serverValidationErrors = error)
      );
  }
}
