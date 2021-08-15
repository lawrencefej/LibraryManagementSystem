import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { EMPTY, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { LoginUserDto } from 'src/dto/models';
import { AuthenticationService } from './_services/authentication.service';
import { SessionService } from './_services/session.service';

@Component({
  selector: 'lms-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private readonly unsubscribe = new Subject<void>();
  private readonly jwtHelper = new JwtHelperService();

  constructor(
    private readonly authenticationService: AuthenticationService,
    private readonly sessionService: SessionService
  ) {}

  ngOnInit(): void {
    // const token = localStorage.getItem('token');
    // const user: User = JSON.parse(localStorage.getItem('user'));
    // if (token) {
    //   this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    // }

    // if (user) {
    //   this.authService.changeUserDetails(user);
    //   this.authService.changeMemberPhoto(user.photoUrl);
    // }
    this.setCurrentUser();
  }

  private setCurrentUser(): void {
    const user: LoginUserDto = JSON.parse(localStorage.getItem('user'));

    if (user) {
      this.authenticationService.setCurrentUser(user);
    }
  }

  private trackTokenRefreshTimer(): void {
    this.sessionService.tokenTimerObservable
      .pipe(
        switchMap(() => {
          if (this.authenticationService.allowTokenRefresh()) {
            console.log('renew token');
            return this.authenticationService.refreshToken();
          } else {
            console.log('ignore');
            return EMPTY;
          }
        }),
        takeUntil(this.unsubscribe)
      )
      .subscribe();
  }
}
