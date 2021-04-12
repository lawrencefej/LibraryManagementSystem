import { BehaviorSubject } from 'rxjs';
import { Checkout } from '../_models/checkout';
import { Injectable } from '@angular/core';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private itemsInBasketSubject: BehaviorSubject<Checkout[]> = new BehaviorSubject(this.getBasketFromLocalStorage());
  private itemsInBasket: Checkout[] = [];

  constructor(private notify: NotificationService) {
    this.itemsInBasketSubject.subscribe(_ => (this.itemsInBasket = _));
  }

  addAssetToCart(checkout: Checkout) {
    const currentItems = [...this.itemsInBasket];
    if (!currentItems.find(x => x.asset.id === checkout.asset.id)) {
      this.itemsInBasketSubject.next([...this.itemsInBasket, checkout]);
      localStorage.setItem('basket', JSON.stringify(this.itemsInBasket));
    } else {
      this.notify.error('This item has already been placed in the basket');
    }
  }

  public getItemsInBasket() {
    return this.itemsInBasketSubject.asObservable();
  }

  public removeFromBasket(id: number) {
    const currentItems = [...this.itemsInBasket];
    const itemsWithoutRemoved = currentItems.filter(_ => _.id !== id);
    this.itemsInBasketSubject.next(itemsWithoutRemoved);
    localStorage.setItem('basket', JSON.stringify(this.itemsInBasket));
  }

  public clearBasket() {
    localStorage.removeItem('basket');
    this.itemsInBasketSubject.next([]);
  }
  getBasketFromLocalStorage() {
    let basket: Checkout[] = [];
    if (localStorage.getItem('basket') !== null) {
      basket = JSON.parse(localStorage.getItem('basket'));
    }
    return basket;
  }
}
