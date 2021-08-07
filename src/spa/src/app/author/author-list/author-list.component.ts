import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge, Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AuthorDto } from 'src/dto/models';
import { AuthorComponent } from '../author/author.component';
import { AuthorService } from '../services/author.service';

@Component({
  selector: 'lms-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css']
})
export class AuthorListComponent implements OnInit, OnDestroy, AfterViewInit {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<AuthorDto>();
  displayedColumns = ['name', 'detail'];
  pagination!: Pagination;
  paginationOptions = new Pagination();
  searchString = new FormControl('');

  constructor(
    private readonly authorService: AuthorService,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly dialog: MatDialog
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getAuthors())
      )
      .subscribe(paginatedCards => {
        this.mapPagination(paginatedCards);
      });
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => this.mapPagination(routeData.initData));

    this.searchAuthors();
  }

  searchAuthors(): void {
    this.searchString.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() => this.getAuthors())
      )
      .subscribe(paginatedResult => this.mapPagination(paginatedResult));
  }

  onSearchClear(): void {
    this.searchString.setValue('');
  }

  openAddAuthorDialog(): void {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(AuthorComponent, dialogConfig);
  }

  private getDialogConfig(): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '500px';

    return dialogConfig;
  }

  private getAuthors(): Observable<PaginatedResult<AuthorDto[]>> {
    return this.authorService.getAuthors(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      this.searchString.value
    );
  }

  private mapPagination(result: PaginatedResult<AuthorDto[]>): void {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }
}
