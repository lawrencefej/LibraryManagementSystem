import { Observable, of } from 'rxjs';
import { Resolve, Router } from '@angular/router';

import { AuthService } from '../_services/auth.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class UserProfileResolver implements Resolve<User> {
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private notify: NotificationService,
    private router: Router
  ) {}

  resolve(): Observable<User> {
    return this.userService.getUser(this.authService.loggedInUser.id).pipe(
      catchError((error) => {
        this.notify.error('Problem retrieving data');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
