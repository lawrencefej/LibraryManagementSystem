import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map, shareReplay, takeUntil } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'lms-responsive-nav',
  templateUrl: './responsive-nav.component.html',
  styleUrls: ['./responsive-nav.component.css']
})
export class ResponsiveNavComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  photoUrl: string;
  loggedInUser: User;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(result => result.matches),
    shareReplay()
  );

  constructor(private breakpointObserver: BreakpointObserver, public authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentPhotoUrl
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(photoUrl => (this.photoUrl = photoUrl));
    this.authService.loggedInUser$.pipe(takeUntil(this.unsubscribe)).subscribe(user => (this.loggedInUser = user));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  logout(): void {
    this.authService.logout();
  }
}
