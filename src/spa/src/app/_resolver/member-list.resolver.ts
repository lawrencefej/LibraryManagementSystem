import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

import { Injectable } from '@angular/core';
import { MemberService } from './../_services/member.service';
import { NotificationService } from '../_services/notification.service';
import { User } from '../_models/user';
import { catchError } from 'rxjs/operators';
import { lmsResolverContants } from './resolver.constants';

@Injectable()
export class MemberListResolver implements Resolve<User[]> {
  constructor(private memberService: MemberService, private router: Router, private notify: NotificationService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.memberService
      .getPaginatedMembers(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '')
      .pipe(
        catchError(() => {
          // TODO Make this central
          this.notify.error('Problem retrieving data');
          this.router.navigate(['/memberSearch']);
          return of(null);
        })
      );
  }
}
