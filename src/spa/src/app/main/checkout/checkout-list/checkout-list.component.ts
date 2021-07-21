import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

import { ActivatedRoute } from '@angular/router';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { merge } from 'rxjs';
import { CheckoutForListDto } from 'src/dto/models/checkout-for-list-dto';

@Component({
  templateUrl: './checkout-list.component.html',
  styleUrls: ['./checkout-list.component.css']
})
export class CheckoutListComponent implements AfterViewInit, OnInit {
  pagination: Pagination;
  checkouts: CheckoutForListDto[] = [];
  dataSource = new MatTableDataSource<CheckoutForListDto>(this.checkouts);
  searchString = '';
  displayedColumns = ['title', 'libraryCardId', 'since', 'until', 'dateReturned', 'status'];
  checkoutFilters = [
    { id: 2, name: 'Checked Out', value: 'checkedOut' },
    { id: 3, name: 'Returned', value: 'returned' },
    { id: 1, name: 'All', value: 'all' }
  ];
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private checkoutService: CheckoutService,
    private route: ActivatedRoute,
    private notify: NotificationService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
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
