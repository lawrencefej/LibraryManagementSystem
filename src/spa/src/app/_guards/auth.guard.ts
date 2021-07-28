import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { AuthService } from '../_services/auth.service';
import { NotificationService } from '../_services/notification.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private notification: NotificationService) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
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

  blockAccess(): boolean {
    this.notification.error('Access Denied');
    this.router.navigate(['/']);
    return false;
  }

  login(state: any): boolean {
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
