import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';

import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { Photo } from 'src/app/_models/photo';
import { PhotoService } from 'src/app/_services/photo.service';
import { User } from 'src/app/_models/user';
import { UserProfileEditComponent } from '../user-profile-edit/user-profile-edit.component';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  @ViewChild('fileInput') myInputVariable: ElementRef;
  user: User;

  constructor(
    private route: ActivatedRoute,
    private photoService: PhotoService,
    private authService: AuthService,
    private notify: NotificationService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.route.data.subscribe(res => {
      this.user = res.user;
    });
  }

  private getDialogConfig() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '440px';

    return dialogConfig;
  }

  public updateProfile() {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = {
      userForUpdate: this.user
    };
    this.dialog
      .open(UserProfileEditComponent, dialogConfig)
      .afterClosed()
      .subscribe(data => {
        if (data) {
          this.user = data;
          this.authService.loggedInUser = this.user;
          this.authService.changeUserDetails(this.user);
          localStorage.setItem('user', JSON.stringify(this.authService.loggedInUser));
        }
      });
  }

  updatePhoto(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const fd = new FormData();
      fd.append('userId', this.user.id.toString());
      fd.append('file', file);
      this.photoService.changeMemberPhoto(fd).subscribe(
        (res: Photo) => {
          this.notify.success('Photo changed successfully');
          this.user.photoUrl = res.url;
          this.authService.changeMemberPhoto(this.user.photoUrl);
          this.authService.loggedInUser.photoUrl = this.user.photoUrl;
          localStorage.setItem('user', JSON.stringify(this.authService.loggedInUser));
        },
        error => {
          this.notify.error(error);
        }
      );
    }
    this.myInputVariable.nativeElement.value = '';
  }
}
