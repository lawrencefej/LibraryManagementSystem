import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge, Observable, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';
import { CheckoutSharedService } from 'src/app/shared/services/checkout-shared.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { CheckoutForListDto, LibraryAssetForDetailedDto } from 'src/dto/models';

@Component({
  selector: 'lms-library-asset-detail-checkout-list',
  templateUrl: './library-asset-detail-checkout-list.component.html',
  styleUrls: ['./library-asset-detail-checkout-list.component.css']
})
export class LibraryAssetDetailCheckoutListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input()
  asset!: LibraryAssetForDetailedDto;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  checkoutFilters = checkoutFilters;
  dataSource = new MatTableDataSource<CheckoutForListDto>();
  displayedColumns = ['title', 'checkoutdate', 'duedate', 'dateReturned', 'status'];
  isCardFormDirty?: boolean;
  isCheckoutTab = false;
  isEditTab = false;
  pagination!: Pagination;
  paginationOptions = new Pagination();
  selected = new FormControl(0);
  selectedFilter = new FormControl(checkoutFilters[0], [Validators.required]);

  constructor(private readonly checkoutSharedService: CheckoutSharedService) {}

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
    this.initializeData();
    this.filterCheckouts();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  private initializeData(): void {
    this.checkoutSharedService
      .getCheckoutsForAsset(
        this.asset.id,
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
  }

  private filterCheckouts(): void {
    this.selectedFilter.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getCheckouts())
      )
      .subscribe(paginatedCheckouts => this.mapPagination(paginatedCheckouts));
  }

  private getCheckouts(): Observable<PaginatedResult<CheckoutForListDto[]>> {
    return this.checkoutSharedService.getCheckoutsForAsset(
      this.asset.id,
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
