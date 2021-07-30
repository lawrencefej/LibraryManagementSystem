import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/_services/notification.service';
import { PhotoService } from 'src/app/_services/photo.service';
import { LibraryAssetForDetailedDto, LibraryAssetStatus } from 'src/dto/models';
import { LibraryAssetService } from '../services/library-asset.service';

@Component({
  templateUrl: './library-asset-detail.component.html',
  styleUrls: ['./library-asset-detail.component.css']
})
export class LibraryAssetDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable?: ElementRef;
  asset!: LibraryAssetForDetailedDto;
  displayedColumns = ['libraryCardId', 'until', 'status'];
  isCardFormDirty = false;
  isEditTab = false;
  selectedTab = new FormControl(0);
  assetStatus = LibraryAssetStatus;

  constructor(
    private photoService: PhotoService,
    private readonly assertService: LibraryAssetService,
    private readonly dialog: MatDialog,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => {
      this.asset = routeData.initData;
      console.log(this.asset);
    });
  }

  tabClicked(tabIndex: number): void {
    this.selectedTab.setValue(tabIndex);
    if (this.selectedTab.value === 2) {
      this.isEditTab = true;
    }
  }

  isFormDirty(isFormDirty: boolean): void {
    this.isCardFormDirty = isFormDirty;
  }

  editCard(): void {
    this.isEditTab = true;
    this.selectedTab.setValue(3);
  }

  cancelEdit(): void {
    if (this.isCardFormDirty) {
      this.notify
        .confirm('Are you sure you want to discard these changes?')
        .afterClosed()
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(response => {
          if (response) {
            this.isEditTab = false;
            this.isCardFormDirty = false;
            this.selectedTab.setValue(0);
          }
        });
    } else {
      this.isEditTab = false;
      this.selectedTab.setValue(0);
    }
  }

  updatePhoto(event: any): void {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const fd = new FormData();
      fd.append('libraryAssetId', this.asset.id.toString());
      fd.append('file', file);
      this.photoService
        .changeMemberPhoto(fd)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(res => {
          this.asset.photoUrl = res.url;
          this.notify.success('Photo changed successfully');
        });
    }
    this.myInputVariable!.nativeElement.value = '';
  }
}
