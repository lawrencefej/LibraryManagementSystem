import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../shared/services/notification.service';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';

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
      catchError(() => {
        this.notify.error('Problem retrieving data');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
