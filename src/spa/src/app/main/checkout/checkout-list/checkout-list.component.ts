import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

import { ActivatedRoute } from '@angular/router';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge, Observable, Subject } from 'rxjs';
import { CheckoutForListDto } from 'src/dto/models/checkout-for-list-dto';
import { switchMap, takeUntil } from 'rxjs/operators';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';
import { FormControl, Validators } from '@angular/forms';

@Component({
  templateUrl: './checkout-list.component.html',
  styleUrls: ['./checkout-list.component.css']
})
export class CheckoutListComponent implements AfterViewInit, OnInit {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  checkoutFilters = checkoutFilters;
  checkouts: CheckoutForListDto[] = [];
  dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
  displayedColumns = ['title', 'libraryCardId', 'since', 'until', 'dateReturned', 'status'];
  pagination: Pagination;
  paginationOptions = new Pagination();
  selectedFilter = new FormControl(checkoutFilters[0], [Validators.required]);

  constructor(private checkoutService: CheckoutService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => {
      this.pagination = data.checkouts.pagination;
      this.checkouts = data.checkouts.result;
      this.dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
    });

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
