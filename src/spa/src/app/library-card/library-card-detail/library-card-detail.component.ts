import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { concatMap, takeUntil } from 'rxjs/operators';
import { BasketViewModel } from 'src/app/main/basket/models/basket-view-model';
import { BasketService } from 'src/app/_services/basket.service';
import { FeeService } from 'src/app/_services/fee.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { PhotoService } from 'src/app/_services/photo.service';
import { LibraryCardForDetailedDto, StateDto } from 'src/dto/models';
import { LibraryCardStatus } from '../Models/library-card-status.enum';

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
    private readonly feeService: FeeService,
    private readonly notify: NotificationService,
    private readonly photoService: PhotoService,
    private readonly route: ActivatedRoute,
    public readonly dialog: MatDialog
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
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const fd = new FormData();
      fd.append('userId', this.card.id.toString());
      fd.append('file', file);
      this.photoService
        .changeMemberPhoto(fd)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(
          res => {
            this.card.photoUrl = res.url;
            this.notify.success('Photo changed successfully');
          },
          error => {
            this.notify.error(error);
          }
        );
    }
    this.myInputVariable.nativeElement.value = '';
  }

  payFees(): void {
    this.notify
      .confirm('Are you sure you want to pay $' + this.card.fees)
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.feeService.payFees(this.card.id);
          }
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
