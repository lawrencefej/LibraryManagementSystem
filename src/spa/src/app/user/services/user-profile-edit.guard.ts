import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { UserProfileComponent } from '../user-profile/user-profile.component';

@Injectable()
export class UserProfileEditGuard implements CanDeactivate<UserProfileComponent> {
  constructor(private readonly notify: NotificationService) {}

  canDeactivate(component: UserProfileComponent): boolean {
    if (component.isFormDirty) {
      return !this.notify.confirm('Are you sure you want to discard these changes?').afterClosed();
    } else {
      return true;
    }
  }
}
