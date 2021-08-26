import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EMPTY, Subject } from 'rxjs';
import { concatMap, takeUntil } from 'rxjs/operators';
import { BasketViewModel } from 'src/app/basket/models/basket-view-model';
import { BasketService } from 'src/app/basket/services/basket.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { LibraryCardForDetailedDto, StateDto } from 'src/dto/models';
import { LibraryCardStatus } from '../models/library-card-status.enum';
import { LibraryCardService } from '../services/library-card.service';

@Component({
  templateUrl: './library-card-detail.component.html',
  styleUrls: ['./library-card-detail.component.css']
})
export class LibraryCardDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable!: ElementRef;

  basket!: BasketViewModel;
  card!: LibraryCardForDetailedDto;
  isCardFormDirty?: boolean;
  isCheckoutTab = false;
  isEditTab = false;
  selectedTab = new FormControl(0);
  states: StateDto[] = [];

  constructor(
    private readonly basketService: BasketService,
    private readonly cardService: LibraryCardService,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute
  ) {
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => {
      this.basket = basket;
      if (this.basket.checkoutComplete) {
        this.selectedTab.setValue(0);
      }
    });
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => {
      this.card = routeData.initData.card;
      this.states = routeData.initData.states;
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  disableCheckouts(): boolean {
    return !this.basket.active || this.basket.libraryCardId !== this.card.id || this.isEditTab;
  }

  isAccountGood(): boolean {
    return this.card.status === LibraryCardStatus.Good && this.card.fees === 0;
  }

  tabClicked(tabIndex: number): void {
    this.selectedTab.setValue(tabIndex);
    if (this.selectedTab.value === 2) {
      this.isEditTab = true;
    }
  }

  isFormDirty(isFormDirty: boolean): void {
    this.isCardFormDirty = isFormDirty;
  }

  startCheckout(): void {
    this.isCheckoutTab = true;
    this.basketService.initializeBasket(this.card);
    this.selectedTab.setValue(1);
  }

  endCheckout(): void {
    this.isCheckoutTab = false;
    this.basketService.clearBasket();
    this.selectedTab.setValue(0);
  }

  editCard(): void {
    this.isEditTab = true;
    this.selectedTab.setValue(3);
  }

  cancelEdit(): void {
    if (this.isCardFormDirty) {
      this.notify
        .confirm('Are you sure you want to discard these changes?')
        .afterClosed()
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(response => {
          if (response) {
            this.isEditTab = false;
            this.isCardFormDirty = false;
            this.selectedTab.setValue(0);
          }
        });
    } else {
      this.isEditTab = false;
      this.selectedTab.setValue(0);
    }
  }

  updatePhoto(event: any): void {
    // TODO validate file ext type
    const file: File = event.target.files[0];

    const formData = new FormData();

    formData.append('file', file, file.name);

    if (file) {
      this.cardService
        .changeCardPhoto(this.card.id, formData)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(photoResponse => {
          this.card.photoUrl = photoResponse.url;
          this.notify.success('Photo changed successfully');
          this.myInputVariable.nativeElement.value = '';
        });
    }
  }

  payFees(): void {
    this.notify
      .confirm('Are you sure you want to pay $' + this.card.fees)
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.cardService.payFees(this.card.id);
          }

          return EMPTY;
        })
      )
      .subscribe(
        () => {
          this.notify.success('Payment was successful');
          this.card.fees = 0;
        },
        error => this.notify.error(error)
      );
  }
}
