import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { EMPTY, Observable, Subject } from 'rxjs';
import { map, shareReplay, switchMap, takeUntil } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { SessionService } from 'src/app/_services/session.service';
import { LoginUserDto } from 'src/dto/models';

@Component({
  selector: 'lms-responsive-nav',
  templateUrl: './responsive-nav.component.html',
  styleUrls: ['./responsive-nav.component.css']
})
export class ResponsiveNavComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  photoUrl!: string;
  loggedInUser!: User;
  user!: LoginUserDto;
  userObservable: Observable<LoginUserDto>;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(result => result.matches),
    shareReplay()
  );

  constructor(
    private readonly authenticationService: AuthenticationService,
    private readonly breakpointObserver: BreakpointObserver,
    private readonly sessionService: SessionService
  ) {}

  ngOnInit(): void {
    this.userObservable = this.authenticationService.loggedInUser$;

    this.trackTokenRefreshTimer();
    this.trackLogoutRefreshTimer();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  logout(): void {
    this.authenticationService.logout().pipe(takeUntil(this.unsubscribe)).subscribe();
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

  private trackLogoutRefreshTimer(): void {
    this.sessionService.logoutTimerObservable
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.authenticationService.logout())
      )
      .subscribe();
  }
}
