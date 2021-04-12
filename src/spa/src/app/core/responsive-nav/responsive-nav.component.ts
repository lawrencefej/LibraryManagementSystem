import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, shareReplay } from 'rxjs/operators';

import { AuthService } from 'src/app/_services/auth.service';
import { Observable } from 'rxjs';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-responsive-nav',
  templateUrl: './responsive-nav.component.html',
  styleUrls: ['./responsive-nav.component.css'],
})
export class ResponsiveNavComponent implements OnInit, OnDestroy {
  photoUrl: string;
  loggedInUser: User;

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay()
    );

  constructor(
    private breakpointObserver: BreakpointObserver,
    public authService: AuthService
  ) {}

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(
      (photoUrl) => (this.photoUrl = photoUrl)
    );
    this.authService.loggedInUser$.subscribe(
      (user) => (this.loggedInUser = user)
    );
  }

  ngOnDestroy() {}

  logout() {
    this.authService.logout();
  }
}
