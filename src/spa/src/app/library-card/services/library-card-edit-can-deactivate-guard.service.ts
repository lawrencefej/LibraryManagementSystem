import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryCardDetailComponent } from '../library-card-detail/library-card-detail.component';

@Injectable()
export class LibraryCardEditCanDeactivateGuardService implements CanDeactivate<LibraryCardDetailComponent> {
  constructor(private readonly notify: NotificationService) {}

  canDeactivate(component: LibraryCardDetailComponent): Observable<boolean> | boolean {
    if (component.isCardFormDirty) {
      return !this.notify.confirm('Are you sure you want to discard these changes?').afterClosed();
    } else {
      return true;
    }
  }
}
