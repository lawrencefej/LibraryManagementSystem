import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserRoles } from 'src/dto/models';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private readonly authenticationService: AuthenticationService, private readonly router: Router) {}

  canActivate(): Observable<boolean> {
    return this.authenticationService.loggedInUser$.pipe(
      map(user => {
        if (user.role === UserRoles.Admin) {
          return true;
        } else {
          this.router.navigateByUrl('/home');
          return false;
        }
      })
    );
  }
}
