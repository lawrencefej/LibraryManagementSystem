import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AuthenticationService } from '../_services/authentication.service';
import { UserService } from '../_services/user.service';

@Injectable()
export class UserProfileResolver implements Resolve<User> {
  constructor(private authService: AuthenticationService, private userService: UserService) {}

  resolve(): Observable<User> {
    return this.userService.getUser(this.authService.userValue.id!);
  }
}
