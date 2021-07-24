import { BehaviorSubject, Observable, Subject } from 'rxjs';
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
  private basketSubject = new BehaviorSubject<BasketViewModel>(this.getDefaultBasket());
  private currentBasket: BasketViewModel;

  basket$: Observable<BasketViewModel> = this.basketSubject.asObservable();

  constructor(private notify: NotificationService) {
    const basketFromLS = JSON.parse(localStorage.getItem('basket'));
    if (basketFromLS) {
      this.basketSubject.next(basketFromLS);
    }
    this.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.currentBasket = basket));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  initializeBasket(card: LibraryCardForDetailedDto): void {
    const basket: BasketViewModel = {
      assets: [],
      cardNumber: card.cardNumber,
      photoUrl: card.photoUrl,
      libraryCardId: card.id,
      active: true,
      checkoutComplete: false
    };

    localStorage.setItem('basket', JSON.stringify(basket));
    this.basketSubject.next(basket);
  }

  addAssetToCart(newAsset: LibraryAssetForListDto, card: LibraryCardForDetailedDto): void {
    if (card.id !== this.currentBasket.libraryCardId) {
      this.notify.error('Please initialize the for for the current card first');
      return;
    }

    const currentBasket = this.currentBasket;
    if (currentBasket.assets.find(asset => asset.libraryAssetId === newAsset.id)) {
      this.notify.error(`${newAsset.title} has already been placed in the basket`);
    } else {
      currentBasket.assets.push({
        title: newAsset.title,
        libraryAssetId: newAsset.id,
        author: newAsset.authorName
      });
      localStorage.setItem('basket', JSON.stringify(currentBasket));
      this.basketSubject.next(currentBasket);
      this.notify.success(`${newAsset.title} was added successfully`);
    }
  }

  removeFromBasket(libraryAssetId: number): void {
    const currentBasket = this.currentBasket;
    const newItemList = currentBasket.assets.filter(item => item.libraryAssetId !== libraryAssetId);
    currentBasket.assets = [...newItemList];
    localStorage.setItem('basket', JSON.stringify(this.currentBasket));
    this.basketSubject.next(currentBasket);
    this.notify.success('Item was removed successfully');
  }

  clearBasket(): void {
    localStorage.removeItem('basket');
    this.basketSubject.next(this.getDefaultBasket());
  }

  completeTransaction(): void {
    const tranactionBasket = this.currentBasket;
    tranactionBasket.checkoutComplete = true;
    this.basketSubject.next(tranactionBasket);
  }

  getDefaultBasket(): BasketViewModel {
    return {
      active: false,
      assets: [],
      cardNumber: '',
      photoUrl: '',
      checkoutComplete: false
    };
  }
}
