import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

import { ActivatedRoute } from '@angular/router';
import { AssetComponent } from '../asset/asset.component';
import { AssetService } from 'src/app/_services/asset.service';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { merge } from 'rxjs';

@Component({
  selector: 'app-asset-list',
  templateUrl: './asset-list.component.html',
  styleUrls: ['./asset-list.component.css'],
})
export class AssetListComponent implements AfterViewInit, OnInit {
  assets: LibraryAsset[] = [];
  pagination: Pagination;
  dataSource = new MatTableDataSource<LibraryAsset>(this.assets);
  searchString = '';
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private assetService: AssetService,
    private route: ActivatedRoute,
    private notify: NotificationService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.pagination = data.assets.pagination;
      this.assets = data.assets.result;
      this.dataSource = new MatTableDataSource<LibraryAsset>(this.assets);
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
      .subscribe((res) => {
        if (res) {
          this.assetService.deleteAsset(asset.id).subscribe(
            () => {
              this.assets.splice(
                this.assets.findIndex((x) => x.id === asset.id),
                1
              );
              this.notify.warn('Item was deleted successfully');
              this.pagination.totalItems--;
              this.dataSource = new MatTableDataSource<LibraryAsset>(
                this.assets
              );
            },
            (error) => {
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
      .subscribe(
        (res: PaginatedResult<LibraryAsset[]>) => {
          this.assets = res.result;
          this.dataSource = new MatTableDataSource<LibraryAsset>(this.assets);
        },
        (error) => {
          this.notify.error(error);
        }
      );
  }
}
