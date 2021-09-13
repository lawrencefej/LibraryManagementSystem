import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import {
  ForgotPasswordRequest,
  LoginUserDto,
  ResetPassword,
  TokenRequestDto,
  TokenResponseDto,
  UserForDetailedDto,
  UserForLoginDto
} from 'src/dto/models';
import { environment } from 'src/environments/environment';
import { SessionService } from './session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  baseUrl = environment.apiUrl + 'auth/';
  private readonly jwtHelper = new JwtHelperService();
  private readonly unsubscribe = new Subject<void>();
  private loggedInUserSubject = new BehaviorSubject<LoginUserDto>({
    email: '',
    firstName: '',
    id: 0,
    lastName: '',
    refreshToken: '',
    role: '',
    token: ''
  });

  loggedInUser$ = this.loggedInUserSubject.asObservable();

  constructor(
    private readonly httpService: HttpClient,
    private readonly router: Router,
    private readonly sessionService: SessionService
  ) {
    this.loggedInUser$.pipe(takeUntil(this.unsubscribe)).subscribe();
  }

  public get userValue(): LoginUserDto {
    return this.loggedInUserSubject.value;
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  login(model: UserForLoginDto): Observable<void> {
    return this.httpService.post<LoginUserDto>(`${this.baseUrl}login`, model).pipe(
      map(response => {
        if (response) {
          this.setCurrentUser(response);
        }
      })
    );
  }

  logout(): Observable<void> {
    return this.httpService.post<void>(`${this.baseUrl}revoke`, this.getTokens()).pipe(
      map(() => {
        this.sessionService.stop();
        localStorage.removeItem('user');
        this.router.navigateByUrl('/auth/login');
      })
    );
  }

  setCurrentUser(user: LoginUserDto): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.loggedInUserSubject.next(user);
    this.sessionService.start();
  }

  updateToken(tokenResponse: TokenResponseDto): void {
    const user: LoginUserDto = this.loggedInUserSubject.value;
    user.token = tokenResponse.token;
    user.refreshToken = tokenResponse.refreshToken;

    this.setCurrentUser(user);
  }

  isLoggedIn(): boolean {
    if (this.loggedInUserSubject.value) {
      return this.loggedInUserSubject.value && !this.jwtHelper.isTokenExpired(this.loggedInUserSubject.value.token!);
    } else {
      return false;
    }
  }

  sendForgotPasswordLink(model: ForgotPasswordRequest): Observable<void> {
    return this.httpService.post<void>(`${this.baseUrl}forgot-password`, model);
  }

  resetPassword(model: ResetPassword): Observable<void> {
    return this.httpService.post<void>(`${this.baseUrl}reset-password`, model);
  }

  refreshToken(): Observable<void> {
    return this.httpService.post<TokenResponseDto>(`${this.baseUrl}refresh`, this.getTokens()).pipe(
      map(tokenResponse => {
        this.updateToken(tokenResponse);
      })
    );
  }

  revokeToken(): Observable<void> {
    return this.httpService.post<void>(`${this.baseUrl}revoke`, this.getTokens());
  }

  changeMemberPhoto(photoUrl: string): void {
    const user: LoginUserDto = this.loggedInUserSubject.value;
    user.photoUrl = photoUrl;

    this.setCurrentUser(user);
  }

  changeUserDetails(newUser: UserForDetailedDto): void {
    const user: LoginUserDto = this.loggedInUserSubject.value;
    user.email = newUser.email;
    user.firstName = newUser.firstName;
    user.lastName = newUser.lastName;
    user.photoUrl = newUser.photoUrl;
    // user.role = newUser.role;
    user.id = newUser.id;
    this.setCurrentUser(user);
  }

  allowTokenRefresh(): boolean {
    if (this.loggedInUserSubject.value.token) {
      return (
        (this.jwtHelper.getTokenExpirationDate(this.loggedInUserSubject.value.token)!.getTime() -
          new Date().getTime()) /
          60000 <=
        5
      );
    } else {
      return false;
    }
  }

  private getTokens(): TokenRequestDto {
    return {
      refreshToken: this.loggedInUserSubject.value.refreshToken!,
      token: this.loggedInUserSubject.value.token!
    };
  }
}
