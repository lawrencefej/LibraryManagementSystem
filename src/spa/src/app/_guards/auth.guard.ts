import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';

import { AuthService } from '../_services/auth.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private notification: NotificationService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    const allowedRoles = next.firstChild.data.allowedRoles as Array<string>;

    if (allowedRoles) {
      if (this.authService.loggedIn) {
        if (this.authService.isAuthorized(allowedRoles)) {
          return true;
        }
        this.blockAccess();
      }
      this.login(state);
    }

    if (this.authService.loggedIn()) {
      return true;
    }

    this.login(state);
  }

  blockAccess() {
    this.notification.error('Access Denied');
    this.router.navigate(['/']);
    return false;
  }

  login(state: any) {
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
