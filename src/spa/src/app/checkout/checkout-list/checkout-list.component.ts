import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge, Observable, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { CheckoutForListDto } from 'src/dto/models';

@Component({
  templateUrl: './checkout-list.component.html',
  styleUrls: ['./checkout-list.component.css']
})
export class CheckoutListComponent implements OnInit, OnDestroy, AfterViewInit {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  checkoutFilters = checkoutFilters;
  checkouts: CheckoutForListDto[] = [];
  dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
  displayedColumns = ['title', 'libraryCardId', 'since', 'until', 'dateReturned', 'status'];
  pagination!: Pagination;
  paginationOptions = new Pagination();
  selectedFilter = new FormControl(checkoutFilters[0], [Validators.required]);

  constructor(private checkoutService: CheckoutService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => {
      this.mapPagination(data.initData);
    });

    this.selectedFilter.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getCheckouts())
      )
      .subscribe(paginatedCheckouts => this.mapPagination(paginatedCheckouts));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
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

  loadData(): void {
    this.getCheckouts()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(paginatedCheckouts => this.mapPagination(paginatedCheckouts));
  }

  private getCheckouts(): Observable<PaginatedResult<CheckoutForListDto[]>> {
    return this.checkoutService.getPaginatedCheckouts(
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
