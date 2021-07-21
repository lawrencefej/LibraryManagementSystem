import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BasketViewModel } from 'src/app/main/basket/models/basket-view-model';
import { BasketService } from 'src/app/_services/basket.service';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { FeeService } from 'src/app/_services/fee.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { PhotoService } from 'src/app/_services/photo.service';
import { CheckoutForListDto, LibraryCardForDetailedDto, StateDto } from 'src/dto/models';
import { LibraryCardComponent } from '../library-card/library-card.component';

@Component({
  templateUrl: './library-card-detail.component.html',
  styleUrls: ['./library-card-detail.component.css']
})
export class LibraryCardDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable: ElementRef;
  basket: BasketViewModel;
  card: LibraryCardForDetailedDto;
  checkouts: CheckoutForListDto[];
  dataSource = new MatTableDataSource<CheckoutForListDto>();
  displayedColumns = ['title', 'duedate', 'status', 'action'];
  isCardFormDirty?: boolean;
  isEditCard = false;
  selected = new FormControl(1);
  states: StateDto[] = [];

  constructor(
    private readonly basketService: BasketService,
    private readonly checkoutService: CheckoutService,
    private readonly feeService: FeeService,
    private readonly notify: NotificationService,
    private readonly photoService: PhotoService,
    private readonly route: ActivatedRoute,
    public readonly dialog: MatDialog
  ) {
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.basket = basket));
  }

  ngOnInit() {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => {
      this.card = routeData.data.card;
      this.checkouts = routeData.data.checkouts;
      this.states = routeData.data.states;
      this.dataSource.data = this.checkouts;
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  isFormDirty(isFormDirty: boolean): void {
    this.isCardFormDirty = isFormDirty;
  }

  startCheckout() {
    this.basketService.initializeBasket(this.card);
  }

  endCheckout() {
    this.basketService.clearBasket();
  }

  public updateMember(element: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';
    dialogConfig.data = element;
    this.dialog.open(LibraryCardComponent, dialogConfig);
  }

  editCard(): void {
    this.isEditCard = true;
    this.selected.setValue(3);
  }

  cancelEdit(): void {
    this.isEditCard = false;
    this.selected.setValue(0);
  }

  updatePhoto(event): void {
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

  // returnAsset(checkout: CheckoutForListDto) {
  //   this.notify
  //     .confirm('Are you sure you want to return ' + checkout.title)
  //     .afterClosed()
  //     .subscribe(res => {
  //       if (res) {
  //         this.checkoutService.returnAsset(checkout.id).subscribe(
  //           () => {
  //             this.notify.success(checkout.title + 'was returned successfully');
  //             this.getCheckoutsForMember();
  //           },
  //           error => {
  //             this.notify.error(error);
  //           }
  //         );
  //       }
  //     });
  // }

  returnAsset(checkout: CheckoutForListDto) {
    this.notify
      .confirm('Are you sure you want to return ' + checkout.title)
      .afterClosed()
      .subscribe(res => {
        if (res) {
          this.checkoutService.returnAsset(checkout.id).subscribe(
            () => {
              this.notify.success(checkout.title + 'was returned successfully');
              // this.getCheckoutsForMember();
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }

  payFees() {
    this.notify.success('fee is paid');
    console.log('paid');

    this.notify
      .confirm('Are you sure you want to pay $' + this.card.fees)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(res => {
        if (res) {
          this.feeService.payFees(this.card.id).subscribe(
            () => {
              this.notify.success('Payment was successful');
              this.card.fees = 0;
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }

  // payFees(card: LibrarycardForListDto) {
  //   this.notify
  //     // .confirm('Are you sure you want to pay $' + card.fees)
  //     .confirm('Are you sure you want to pay $')
  //     .afterClosed()
  //     .pipe(takeUntil(this.unsubscribe))
  //     .subscribe(res => {
  //       if (res) {
  //         this.feeService.payFees(card.id).subscribe(
  //           () => {
  //             this.notify.success('Payment was successful');
  //             this.card.fees = 0;
  //           },
  //           error => {
  //             this.notify.error(error);
  //           }
  //         );
  //       }
  //     });
  // }
}
