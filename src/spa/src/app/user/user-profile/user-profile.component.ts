import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UserForDetailedDto } from 'src/dto/models';
import { UserService } from '../services/user.service';

@Component({
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('fileInput') myInputVariable!: ElementRef;

  user!: UserForDetailedDto;

  constructor(
    private readonly userService: UserService,
    private readonly route: ActivatedRoute,
    // private readonly authService: AuthenticationService,
    private readonly notify: NotificationService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => (this.user = routeData.initData));
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
