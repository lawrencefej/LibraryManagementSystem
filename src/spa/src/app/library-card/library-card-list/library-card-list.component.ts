import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { EMPTY, merge, Observable, Subject } from 'rxjs';
import { concatMap, debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibrarycardForListDto } from 'src/dto/models';
import { LibraryCardComponent } from '../library-card/library-card.component';
import { LibraryCardService } from '../services/library-card.service';

@Component({
  templateUrl: './library-card-list.component.html',
  styleUrls: ['./library-card-list.component.css']
})
export class LibraryCardListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<LibrarycardForListDto>();
  displayedColumns = ['libraryCardNumber', 'firstName', 'lastName', 'email', 'detail', 'delete'];
  pagination!: Pagination;
  paginationOptions = new Pagination();
  searchString = new FormControl('');

  constructor(
    private readonly activatedroute: ActivatedRoute,
    private readonly cardService: LibraryCardService,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly dialog: MatDialog
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.activatedroute.queryParams.pipe(takeUntil(this.unsubscribe)).subscribe(params => {
      if (params.new) {
        this.openAddCardDialog();
      }
    });
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => this.mapPagination(routeData.initData));

    this.searchCards();
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getCards())
      )
      .subscribe(paginatedCards => {
        this.mapPagination(paginatedCards);
      });
  }

  searchCards(): void {
    this.searchString.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() => this.getCards())
      )
      .subscribe(paginatedResult => this.mapPagination(paginatedResult));
  }

  onSearchClear(): void {
    this.searchString.setValue('');
  }

  openAddCardDialog(): void {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(LibraryCardComponent, dialogConfig);
  }

  // TODO Deactivate card instead & fix server error
  deleteCard(card: LibrarycardForListDto): void {
    this.notify
      .confirm('Are you sure you sure you want to delete this member')
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.cardService.deleteCard(card.id);
          }
          return EMPTY;
        })
      )
      .subscribe(() => {
        this.notify.success('Member was deleted successfully');
        this.pagination.totalItems--;
      });
  }

  private getDialogConfig(): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';

    return dialogConfig;
  }

  private getCards(): Observable<PaginatedResult<LibrarycardForListDto[]>> {
    return this.cardService.getCards(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      this.searchString.value
    );
  }

  private mapPagination(result: PaginatedResult<LibrarycardForListDto[]>): void {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }
}
