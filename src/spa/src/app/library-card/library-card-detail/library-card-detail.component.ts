import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge, Observable, Subject } from 'rxjs';
import { concatMap, takeUntil } from 'rxjs/operators';
import { BasketViewModel } from 'src/app/main/basket/models/basket-view-model';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';
import { Pagination } from 'src/app/_models/pagination';
import { BasketService } from 'src/app/_services/basket.service';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { FeeService } from 'src/app/_services/fee.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { PhotoService } from 'src/app/_services/photo.service';
import { CheckoutForListDto, LibraryCardForDetailedDto, StateDto } from 'src/dto/models';

@Component({
  templateUrl: './library-card-detail.component.html',
  styleUrls: ['./library-card-detail.component.css']
})
export class LibraryCardDetailComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable: ElementRef;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  basket: BasketViewModel;
  card: LibraryCardForDetailedDto;
  checkoutFilters = checkoutFilters;
  checkouts: CheckoutForListDto[];
  dataSource = new MatTableDataSource<CheckoutForListDto>();
  displayedColumns = ['title', 'duedate', 'dateReturned', 'status', 'action', 'renew'];
  isCardFormDirty?: boolean;
  isEditCard = false;
  paginationOptions = new Pagination();
  selectedFilter = new FormControl(checkoutFilters[0], [Validators.required]);
  selected = new FormControl(0);
  states: StateDto[] = [];
  pagination: Pagination;

  constructor(
    private readonly basketService: BasketService,
    private readonly checkoutService: CheckoutService,
    private readonly feeService: FeeService,
    private readonly notify: NotificationService,
    private readonly photoService: PhotoService,
    private readonly route: ActivatedRoute,
    public readonly dialog: MatDialog
  ) {
    this.selectedFilter.valueChanges.pipe(takeUntil(this.unsubscribe)).subscribe(() => this.loadData());
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.basket = basket));
  }

  ngOnInit() {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => {
      this.card = routeData.data.card;
      this.pagination = routeData.data.checkouts.pagination;
      this.checkouts = routeData.data.checkouts.result;
      this.states = routeData.data.states;
      this.dataSource.data = this.checkouts;
    });
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange).subscribe(() => {
      this.loadData();
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

  loadData(): void {
    this.checkoutService
      .getPaginatedCheckoutsForCard(
        this.card.id,
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        this.selectedFilter.value
      )
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);

          this.checkouts = response.result;
          this.pagination = response.pagination;
          this.dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
        },
        error => {
          this.notify.error(error);
        }
      );
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

  renewCheckout(checkout: CheckoutForListDto): void {
    this.notify
      .confirm('Are you sure you want to renew ' + checkout.title)
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.updateCheckout(checkout, true);
          }
        })
      )
      .subscribe(
        () => {
          this.notify.success(checkout.title + 'was renewed successfully');
          this.loadData();
        },
        error => {
          this.notify.error(error);
        }
      );
  }

  returnCheckout(checkout: CheckoutForListDto): void {
    this.notify
      .confirm('Are you sure you want to return ' + checkout.title)
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.updateCheckout(checkout);
          }
        })
      )
      .subscribe(
        () => {
          this.notify.success(checkout.title + 'was returned successfully');
          this.loadData();
        },
        error => {
          this.notify.error(error);
        }
      );
  }

  private updateCheckout(checkout: CheckoutForListDto, isRenew = false): Observable<void> {
    return this.checkoutService.checkInAsset({
      checkoutId: checkout.id,
      isRenew
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
}
