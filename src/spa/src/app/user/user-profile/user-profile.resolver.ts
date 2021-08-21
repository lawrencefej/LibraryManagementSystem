import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { UserForDetailedDto } from 'src/dto/models';
import { UserService } from '../services/user.service';

@Injectable()
export class UserProfileResolver implements Resolve<UserForDetailedDto> {
  constructor(private readonly userService: UserService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<UserForDetailedDto> {
    return this.userService.getUser(route.params.id);
  }
}
