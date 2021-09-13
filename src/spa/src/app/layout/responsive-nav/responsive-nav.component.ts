import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { EMPTY, Observable, Subject } from 'rxjs';
import { map, shareReplay, switchMap, takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/_services/authentication.service';
import { SessionService } from 'src/app/_services/session.service';
import { LoginUserDto } from 'src/dto/models';

@Component({
  selector: 'lms-responsive-nav',
  templateUrl: './responsive-nav.component.html',
  styleUrls: ['./responsive-nav.component.css']
})
export class ResponsiveNavComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  userObservable: Observable<LoginUserDto> = this.authService.loggedInUser$;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(result => result.matches),
    shareReplay()
  );

  constructor(
    private readonly authService: AuthService,
    private readonly breakpointObserver: BreakpointObserver,
    private readonly sessionService: SessionService
  ) {}

  ngOnInit(): void {
    this.trackTokenRefreshTimer();
    this.trackLogoutRefreshTimer();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  logout(): void {
    this.authService.logout().pipe(takeUntil(this.unsubscribe)).subscribe();
  }

  private trackTokenRefreshTimer(): void {
    this.sessionService.tokenTimerObservable
      .pipe(
        switchMap(() => {
          if (this.authService.allowTokenRefresh()) {
            return this.authService.refreshToken();
          } else {
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
        switchMap(() => this.authService.logout())
      )
      .subscribe();
  }
}
