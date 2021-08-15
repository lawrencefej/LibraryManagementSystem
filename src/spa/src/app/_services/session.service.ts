import { Injectable } from '@angular/core';
import { Observable, Subject, timer } from 'rxjs';
import { map, repeatWhen, takeUntil } from 'rxjs/operators';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  tokenTimerObservable: Observable<number>;
  logoutTimerObservable: Observable<void>;
  private readonly startTimer = new Subject<void>();
  private readonly stopTimer = new Subject<void>();
  private readonly startLogoutTimer = new Subject<void>();
  private readonly stopLogoutTimer = new Subject<void>();

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
      map(count => {
        console.log(count, 'logout');
      }),
      takeUntil(this.stopLogoutTimer),
      repeatWhen(() => this.startLogoutTimer)
    );
  }

  private initializeTokenTimer(): void {
    this.tokenTimerObservable = timer(this.appConfig.tokenInterval, this.appConfig.tokenInterval).pipe(
      map(test => {
        console.log(test);
        return test;
      }),
      takeUntil(this.stopTimer),
      repeatWhen(() => this.startTimer)
    );
  }
}
