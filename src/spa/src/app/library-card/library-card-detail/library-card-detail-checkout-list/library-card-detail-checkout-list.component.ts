import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge, Observable, Subject } from 'rxjs';
import { concatMap, switchMap, takeUntil } from 'rxjs/operators';
import { BasketViewModel } from 'src/app/main/basket/models/basket-view-model';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { BasketService } from 'src/app/_services/basket.service';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { CheckoutForListDto, LibraryCardForDetailedDto } from 'src/dto/models';

@Component({
  selector: 'lms-library-card-detail-checkout-list',
  templateUrl: './library-card-detail-checkout-list.component.html',
  styleUrls: ['./library-card-detail-checkout-list.component.css']
})
export class LibraryCardDetailCheckoutListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input()
  card: LibraryCardForDetailedDto;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  basket: BasketViewModel;
  checkoutFilters = checkoutFilters;
  dataSource = new MatTableDataSource<CheckoutForListDto>();
  displayedColumns = ['title', 'checkoutdate', 'duedate', 'dateReturned', 'status', 'return', 'renew'];
  isCardFormDirty?: boolean;
  isEditTab = false;
  isCheckoutTab = false;
  paginationOptions = new Pagination();
  selectedFilter = new FormControl(checkoutFilters[0], [Validators.required]);
  selected = new FormControl(0);
  pagination: Pagination;

  constructor(
    private readonly basketService: BasketService,
    private readonly checkoutService: CheckoutService,
    private readonly notify: NotificationService
  ) {
    this.selectedFilter.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getCheckouts())
      )
      .subscribe(paginatedCheckouts => this.mapPagination(paginatedCheckouts));
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getCheckouts())
      )
      .subscribe(paginatedCheckouts => {
        this.mapPagination(paginatedCheckouts);
      });
  }

  ngOnInit(): void {
    this.checkoutService
      .getPaginatedCheckoutsForCard(
        this.card.id,
        lmsResolverContants.pageNumber,
        lmsResolverContants.pageSize,
        '',
        '',
        checkoutFilters[0]
      )
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(response => {
        this.mapPagination(response);
      });

    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => {
      this.basket = basket;
      if (this.basket.checkoutComplete) {
        this.loadData();
      }
    });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  loadData(): void {
    this.getCheckouts()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(paginatedCheckouts => this.mapPagination(paginatedCheckouts));
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
      .subscribe(() => {
        this.notify.success(checkout.title + 'was renewed successfully');
        this.loadData();
      });
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
      .subscribe(() => {
        this.notify.success(checkout.title + 'was returned successfully');
        this.loadData();
      });
  }

  private updateCheckout(checkout: CheckoutForListDto, isRenew: boolean = false): Observable<void> {
    return this.checkoutService.checkInAsset({
      checkoutId: checkout.id,
      isRenew
    });
  }

  private getCheckouts(): Observable<PaginatedResult<CheckoutForListDto[]>> {
    return this.checkoutService.getPaginatedCheckoutsForCard(
      this.card.id,
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      this.selectedFilter.value
    );
  }

  private mapPagination(result: PaginatedResult<CheckoutForListDto[]>): void {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }
}
