import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { AssetComponent } from '../asset/asset.component';
import { Checkout } from 'src/app/_models/checkout';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { MatTableDataSource } from '@angular/material/table';
import { NotificationService } from 'src/app/_services/notification.service';
import { Photo } from 'src/app/_models/photo';
import { PhotoService } from 'src/app/_services/photo.service';

@Component({
  selector: 'app-asset-detail',
  templateUrl: './asset-detail.component.html',
  styleUrls: ['./asset-detail.component.css']
})
export class AssetDetailComponent implements OnInit {
  @ViewChild('fileInput') myInputVariable: ElementRef;
  displayedColumns = ['libraryCardId', 'until', 'status'];
  asset: LibraryAsset;
  checkouts: Checkout[];
  dataSource = new MatTableDataSource<Checkout>();
  constructor(
    private notify: NotificationService,
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private checkoutService: CheckoutService,
    private photoService: PhotoService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(asset => {
      this.asset = asset.asset;
    });
    this.getCheckoutsForAsset();
  }

  getCheckoutsForAsset() {
    this.checkoutService.getCheckoutsForAsset(this.asset.id).subscribe(
      (checkouts: Checkout[]) => {
        this.checkouts = checkouts;
        this.dataSource.data = checkouts;
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  public updateAsset(element: any) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';
    dialogConfig.data = element;
    this.dialog.open(AssetComponent, dialogConfig);
  }

  updatePhoto(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const fd = new FormData();
      fd.append('libraryAssetId', this.asset.id.toString());
      fd.append('file', file);
      this.photoService.changeAssetPhoto(fd).subscribe(
        (res: Photo) => {
          this.asset.photoUrl = res.url;
          this.notify.success('Photo changed successfully');
        },
        error => {
          this.notify.error(error);
        }
      );
    }
    this.myInputVariable.nativeElement.value = '';
  }
}
