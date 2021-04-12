import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { AssetComponent } from '../../libraryAssets/asset/asset.component';
import { AssetService } from 'src/app/_services/asset.service';
import { Author } from 'src/app/_models/author';
import { AuthorService } from 'src/app/_services/author.service';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { Pagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-author-asset',
  templateUrl: './author-asset.component.html',
  styleUrls: ['./author-asset.component.css'],
})
export class AuthorAssetComponent implements OnInit {
  author: Author;
  assets: LibraryAsset[] = [];
  dataSource = new MatTableDataSource<LibraryAsset>(this.assets);
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private authorService: AuthorService,
    private route: ActivatedRoute,
    private notify: NotificationService,
    public dialog: MatDialog,
    private assetService: AssetService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.author = data.author;
    });
    this.getAssets();
  }

  getAssets() {
    this.authorService.getAssetForAuthor(this.author.id).subscribe(
      (assets: LibraryAsset[]) => {
        this.assets = assets;
        this.dataSource = new MatTableDataSource<LibraryAsset>(this.assets);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      (error) => {
        this.notify.error(error);
      }
    );
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openAddAssetDialog() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';
    dialogConfig.data = {
      author: this.author,
    };
    this.dialog.open(AssetComponent, dialogConfig);
  }

  public updateAsset(element: any) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';
    dialogConfig.data = element;
    this.dialog.open(AssetComponent, dialogConfig);
  }

  onSearchClear() {
    this.dataSource.paginator.firstPage();
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
              this.dataSource = new MatTableDataSource<LibraryAsset>(
                this.assets
              );
              this.dataSource.paginator = this.paginator;
              this.dataSource.sort = this.sort;
            },
            (error) => {
              this.notify.error(error);
            }
          );
        }
      });
  }
}
