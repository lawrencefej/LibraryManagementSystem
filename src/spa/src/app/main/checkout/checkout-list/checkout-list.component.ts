import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

import { ActivatedRoute } from '@angular/router';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { merge, Subject } from 'rxjs';
import { CheckoutForListDto } from 'src/dto/models/checkout-for-list-dto';
import { takeUntil } from 'rxjs/operators';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';

@Component({
  templateUrl: './checkout-list.component.html',
  styleUrls: ['./checkout-list.component.css']
})
export class CheckoutListComponent implements AfterViewInit, OnInit {
  private readonly unsubscribe = new Subject<void>();

  pagination: Pagination;
  checkouts: CheckoutForListDto[] = [];
  dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
  searchString = '';
  displayedColumns = ['title', 'libraryCardId', 'since', 'until', 'dateReturned', 'status'];
  checkoutFilters = checkoutFilters;
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private checkoutService: CheckoutService,
    private route: ActivatedRoute,
    private notify: NotificationService
  ) {}

  ngOnInit() {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => {
      this.pagination = data.checkouts.pagination;
      this.checkouts = data.checkouts.result;
      this.dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
    });
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange).subscribe(() => {
      this.loadData();
    });
  }

  filterList(value: string) {
    this.searchString = value;
    this.loadData();
  }

  loadData() {
    this.checkoutService
      .getPaginatedCheckouts(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        this.searchString
      )
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        (res: PaginatedResult<CheckoutForListDto[]>) => {
          this.checkouts = res.result;
          this.pagination = res.pagination;
          this.dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
        },
        error => {
          this.notify.error(error);
        }
      );
  }
}
