import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../_services/authentication.service';
import { SessionService } from '../_services/session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private readonly sessionService: SessionService
  ) {}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authenticationService.loggedInUser$.pipe(
      map(user => {
        if (user) {
          this.sessionService.resetLogoutTimer();
          return true;
        } else {
          this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
          return false;
        }
      })
    );
  }
}
