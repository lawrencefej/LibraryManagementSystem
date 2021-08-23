import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UserForDetailedDto, UserForUpdateDto } from 'src/dto/models';
import { UserService } from '../services/user.service';

@Component({
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable!: ElementRef;

  isEditTab = false;
  isFormDirty = false;
  selectedTab = new FormControl(0);
  user!: UserForDetailedDto;

  constructor(
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly userService: UserService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => (this.user = routeData.initData));
  }

  tabClicked(tabIndex: number): void {
    this.selectedTab.setValue(tabIndex);
    if (this.selectedTab.value === 1) {
      this.isEditTab = true;
    } else {
      this.isEditTab = false;
    }
  }

  editUser(): void {
    this.isEditTab = true;
    this.selectedTab.setValue(1);
  }

  cancelEdit(): void {
    if (this.isFormDirty) {
      this.notify
        .confirm('Are you sure you want to discard these changes?')
        .afterClosed()
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(response => {
          if (response) {
            this.isEditTab = false;
            this.isFormDirty = false;
            this.selectedTab.setValue(0);
          }
        });
    } else {
      this.isEditTab = false;
      this.selectedTab.setValue(0);
    }
  }

  updateProfile(model: UserForUpdateDto): void {
    this.userService
      .updatedUserProfile(model)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(returnUser => {
        this.user = returnUser;
        this.notify.success('Profile updated successfully');
      });
  }

  updatePhoto(files: File[]): void {
    // TODO validate file ext type
    const file: File = files[0];

    const formData = new FormData();

    formData.append('file', file, file.name);

    if (file) {
      this.userService
        .changeUserPhoto(this.user.id, formData)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(photoResponse => {
          this.user.photoUrl = photoResponse.url;
          this.notify.success('Photo changed successfully');
          this.myInputVariable.nativeElement.value = '';
        });
    }
  }
}
