import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { LibraryAssetForDetailedDto, LibraryAssetStatus } from 'src/dto/models';
import { LibraryAssetService } from '../services/library-asset.service';

@Component({
  templateUrl: './library-asset-detail.component.html',
  styleUrls: ['./library-asset-detail.component.css']
})
export class LibraryAssetDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable!: ElementRef;

  asset!: LibraryAssetForDetailedDto;
  assetStatus = LibraryAssetStatus;
  displayedColumns = ['libraryCardId', 'until', 'status'];
  isCardFormDirty = false;
  isEditTab = false;
  selectedTab = new FormControl(0);

  constructor(
    private readonly assetService: LibraryAssetService,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => (this.asset = routeData.initData));
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
    // TODO validate file ext type
    const file: File = event.target.files[0];

    const formData = new FormData();

    formData.append('file', file, file.name);

    if (file) {
      this.assetService
        .changeAssetPhoto(this.asset.id, formData)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(photoResponse => {
          this.asset.photoUrl = photoResponse.url;
          this.notify.success('Photo changed successfully');
          this.myInputVariable.nativeElement.value = '';
        });
    }
  }
}
