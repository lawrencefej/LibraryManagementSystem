import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators/';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseurl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  loggedInUser: User;
  decodedToken: any;
  private loggedInUserSubject = new BehaviorSubject<User>(null);
  loggedInUser$ = this.loggedInUserSubject.asObservable();
  private photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.loggedInUser$.subscribe((user) => (this.loggedInUser = user));
  }

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  changeUserDetails(user: User) {
    this.loggedInUserSubject.next(user);
  }

  login(model: any) {
    return this.http.post(this.baseurl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user && user.token) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.changeUserDetails(user.user);
          this.changeMemberPhoto(this.loggedInUser.photoUrl);
        }

        return user.user;
      })
    );
  }

  register(user: User) {
    return this.http.post(this.baseurl + 'auth/register', user);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.decodedToken = null;
    this.router.navigate(['/login']);
  }

  sendForgotPasswordLink(model: any) {
    return this.http.post(this.baseurl + 'forgot-password', model);
  }

  resetPassword(model: any) {
    return this.http.post(this.baseurl + 'reset-password', model);
  }

  isAuthorized(allowedRoles: string[]) {
    // get token from local storage or state management
    const token = localStorage.getItem('token');

    // decode token to read the payload details
    const decodeToken = this.jwtHelper.decodeToken(token);

    // check if it was decoded successfully, if not the token is not valid, deny access
    if (!decodeToken) {
      console.log('Invalid token');
      return false;
    }

    // check if the user roles is in the list of allowed roles, return true if allowed and false if not allowed
    return allowedRoles.includes(decodeToken.role);
  }
}
