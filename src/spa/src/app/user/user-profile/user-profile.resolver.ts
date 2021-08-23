import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/app/_services/authentication.service';
import { UserForDetailedDto } from 'src/dto/models';
import { UserService } from '../services/user.service';

@Injectable()
export class UserProfileResolver implements Resolve<UserForDetailedDto> {
  constructor(private readonly authService: AuthenticationService, private readonly userService: UserService) {}

  resolve(): Observable<UserForDetailedDto> {
    return this.userService.getUser(this.authService.userValue.id);
  }
}
