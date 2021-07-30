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
import { LibraryAssetForListDto } from 'src/dto/models';
import { LibraryAssetComponent } from '../library-asset/library-asset.component';
import { LibraryAssetService } from '../services/library-asset.service';

@Component({
  templateUrl: './library-asset-list.component.html',
  styleUrls: ['./library-asset-list.component.css']
})
export class LibraryAssetListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  dataSource = new MatTableDataSource<LibraryAssetForListDto>();
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'detail', 'delete'];
  pagination!: Pagination;
  paginationOptions = new Pagination();
  searchString = new FormControl('');

  constructor(
    private readonly assetService: LibraryAssetService,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly dialog: MatDialog
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => this.mapPagination(routeData.initData));

    this.searchAssets();
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getAssets())
      )
      .subscribe(paginatedCards => {
        this.mapPagination(paginatedCards);
      });
  }

  searchAssets(): void {
    this.searchString.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() => this.getAssets())
      )
      .subscribe(paginatedResult => this.mapPagination(paginatedResult));
  }

  onSearchClear(): void {
    this.searchString.setValue('');
  }

  openAddAssetDialog(): void {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(LibraryAssetComponent, dialogConfig);
  }

  deleteAsset(asset: LibraryAssetForListDto): void {
    this.notify
      .confirm('Are you sure you sure you want to delete this item')
      .afterClosed()
      .pipe(
        takeUntil(this.unsubscribe),
        concatMap(response => {
          if (response) {
            return this.assetService.deleteAsset(asset.id);
          }

          return EMPTY;
        })
      )
      .subscribe(() => {
        this.notify.success('Item was deleted successfully');
        this.pagination.totalItems--;
      });
  }

  private getDialogConfig(): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';

    return dialogConfig;
  }

  private getAssets(): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    return this.assetService.getAssets(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      this.searchString.value
    );
  }

  private mapPagination(result: PaginatedResult<LibraryAssetForListDto[]>): void {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }
}
