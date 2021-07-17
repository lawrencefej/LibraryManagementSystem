import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Observable, of, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BasketService } from 'src/app/_services/basket.service';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { FeeService } from 'src/app/_services/fee.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { PhotoService } from 'src/app/_services/photo.service';
import {
  BasketForCheckoutDto,
  CheckoutForListDto,
  LibraryCardForDetailedDto,
  LibrarycardForListDto
} from 'src/dto/models';
import { LibraryCardComponent } from '../library-card/library-card.component';

@Component({
  selector: 'app-library-card-detail',
  templateUrl: './library-card-detail.component.html',
  styleUrls: ['./library-card-detail.component.css']
})
export class LibraryCardDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable: ElementRef;
  displayedColumns = ['title', 'duedate', 'status', 'action'];
  card: LibraryCardForDetailedDto;
  checkouts: CheckoutForListDto[];
  dataSource = new MatTableDataSource<CheckoutForListDto>();
  public basketItems$: Observable<CheckoutForListDto[]> = of([]);
  public basketItems: BasketForCheckoutDto[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    public readonly dialog: MatDialog,
    private readonly notify: NotificationService,
    private readonly photoService: PhotoService,
    private readonly feeService: FeeService,
    private readonly basketService: BasketService,
    private readonly checkoutService: CheckoutService
  ) {}

  ngOnInit() {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => {
      this.card = data.card;
      this.checkouts = data.checkouts;
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  public updateMember(element: any): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';
    dialogConfig.data = element;
    this.dialog.open(LibraryCardComponent, dialogConfig);
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

  payFees(card: LibrarycardForListDto) {
    this.notify
      // .confirm('Are you sure you want to pay $' + card.fees)
      .confirm('Are you sure you want to pay $')
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(res => {
        if (res) {
          this.feeService.payFees(card.id).subscribe(
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
}
