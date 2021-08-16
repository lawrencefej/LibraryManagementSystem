import { Injectable } from '@angular/core';
import { Observable, Subject, timer } from 'rxjs';
import { repeatWhen, takeUntil } from 'rxjs/operators';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private readonly startLogoutTimer = new Subject<void>();
  private readonly startTimer = new Subject<void>();
  private readonly stopLogoutTimer = new Subject<void>();
  private readonly stopTimer = new Subject<void>();

  logoutTimerObservable!: Observable<number>;
  tokenTimerObservable!: Observable<number>;

  constructor(private readonly appConfig: AppConfigService) {
    this.initializeTokenTimer();
    this.initializeLogoutTimer();
  }

  start(): void {
    this.startTimer.next();
  }

  stop(): void {
    this.stopTimer.next();
  }

  startLogout(): void {
    this.startLogoutTimer.next();
  }

  stopLogout(): void {
    this.stopLogoutTimer.next();
  }

  resetLogoutTimer(): void {
    this.stopLogoutTimer.next();
    this.startLogoutTimer.next();
  }

  private initializeLogoutTimer(): void {
    // TODO Reset on tabs
    this.logoutTimerObservable = timer(this.appConfig.logoutInterval, this.appConfig.logoutInterval).pipe(
      takeUntil(this.stopLogoutTimer),
      repeatWhen(() => this.startLogoutTimer)
    );
  }

  private initializeTokenTimer(): void {
    this.tokenTimerObservable = timer(this.appConfig.tokenInterval, this.appConfig.tokenInterval).pipe(
      takeUntil(this.stopTimer),
      repeatWhen(() => this.startTimer)
    );
  }
}
