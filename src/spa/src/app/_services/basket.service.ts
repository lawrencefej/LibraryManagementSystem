import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Checkout } from '../_models/checkout';
import { Injectable, OnDestroy } from '@angular/core';
import { NotificationService } from './notification.service';
import { BasketViewModel } from '../main/basket/models/basket-view-model';
import { LibraryAssetForListDto, LibraryCardForDetailedDto } from 'src/dto/models';
import { takeUntil } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BasketService implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();
  private basketSubject = new BehaviorSubject<BasketViewModel>(undefined);
  private currentBasket: BasketViewModel;

  basket$: Observable<BasketViewModel> = this.basketSubject.asObservable();

  constructor(private notify: NotificationService) {
    this.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.currentBasket = basket));
    this.basketSubject.next(JSON.parse(localStorage.getItem('basket')));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  initializeBasket(card: LibraryCardForDetailedDto) {
    const basket: BasketViewModel = {
      assets: [],
      cardNumber: card.cardNumber,
      photoUrl: card.photoUrl,
      libraryCardId: card.id,
      active: true
    };

    localStorage.setItem('basket', JSON.stringify(basket));
    this.basketSubject.next(basket);
  }

  addAssetToCart(newAsset: LibraryAssetForListDto) {
    const currentBasket = this.currentBasket;
    if (currentBasket.assets.find(asset => asset.libraryAssetId === newAsset.id)) {
      this.notify.error(`${newAsset.title} has already been placed in the basket`);
    } else {
      this.currentBasket.assets.push({
        title: newAsset.title,
        libraryAssetId: newAsset.id,
        author: newAsset.authorName
      });
      this.basketSubject.next(this.currentBasket);
      localStorage.setItem('basket', JSON.stringify(this.currentBasket));
      this.notify.success(`${newAsset.title} was added successfully`);
    }
  }

  removeFromBasket(libraryAssetId: number) {
    const currentItems = this.currentBasket;
    const newItemList = currentItems.assets.filter(item => item.libraryAssetId !== libraryAssetId);
    this.currentBasket.assets = [...newItemList];
    this.basketSubject.next(currentItems);
    localStorage.setItem('basket', JSON.stringify(this.currentBasket));
  }

  getBasketFromLocalStorage() {
    let basket: Checkout[] = [];
    if (localStorage.getItem('basket') !== null) {
      basket = JSON.parse(localStorage.getItem('basket'));
    }
    return basket;
  }

  clearBasket() {
    localStorage.removeItem('basket');
    this.basketSubject.next(undefined);
  }
}
