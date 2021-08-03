import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AssetService } from 'src/app/_services/asset.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryAssetForListDto } from 'src/dto/models/library-asset-for-list-dto';
import { AssetComponent } from '../asset/asset.component';

@Component({
  templateUrl: './asset-list.component.html',
  styleUrls: ['./asset-list.component.css']
})
export class AssetListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  assets: LibraryAssetForListDto[] = [];
  pagination!: Pagination;
  dataSource = new MatTableDataSource<LibraryAssetForListDto>(this.assets);
  searchString = '';
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];
  paginationOptions = new Pagination();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private assetService: AssetService,
    private route: ActivatedRoute,
    private notify: NotificationService,
    public dialog: MatDialog
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit() {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => {
      this.pagination = data.assets.pagination;
      this.assets = data.assets.result;
      this.dataSource = new MatTableDataSource<LibraryAssetForListDto>(this.assets);
    });
  }

  ngAfterViewInit() {
    merge(this.paginator.page, this.sort.sortChange).subscribe(() => {
      this.loadData();
    });
  }

  filterList() {
    this.searchString.trim().toLocaleLowerCase();
    this.loadData();
  }

  onSearchClear() {
    this.searchString = '';
    this.filterList();
  }

  private getDialogConfig() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';

    return dialogConfig;
  }

  public updateAsset(element: any) {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = element;
    this.dialog.open(AssetComponent, dialogConfig);
  }

  openAddAssetDialog() {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(AssetComponent, dialogConfig);
  }

  deleteAsset(asset: LibraryAsset) {
    this.notify
      .confirm('Are you sure you sure you want to delete this item')
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(res => {
        if (res) {
          this.assetService
            .deleteAsset(asset.id)
            .pipe(takeUntil(this.unsubscribe))
            .subscribe(
              () => {
                this.assets.splice(
                  this.assets.findIndex(x => x.id === asset.id),
                  1
                );
                this.notify.warn('Item was deleted successfully');
                this.pagination.totalItems--;
                this.dataSource = new MatTableDataSource<LibraryAssetForListDto>(this.assets);
              },
              error => {
                this.notify.error(error);
              }
            );
        }
      });
  }

  loadData() {
    this.assetService
      .getPaginatedAssets(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        this.searchString
      )
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        (res: PaginatedResult<LibraryAssetForListDto[]>) => {
          this.assets = res.result;
          this.dataSource = new MatTableDataSource<LibraryAssetForListDto>(this.assets);
        },
        error => {
          this.notify.error(error);
        }
      );
  }
}
