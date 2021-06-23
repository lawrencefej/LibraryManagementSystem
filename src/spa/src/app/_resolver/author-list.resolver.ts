import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

import { Author } from '../_models/author';
import { AuthorService } from '../_services/author.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { catchError } from 'rxjs/operators';
import { lmsResolverContants } from './resolver.constants';

@Injectable()
export class AuthorListResolver implements Resolve<Author[]> {
  constructor(
    private authorService: AuthorService,
    private notify: NotificationService,
    private router: Router
  ) {}

  resolve(): Observable<Author[]> {
    return this.authorService
      .getPaginatedAuthors(
        lmsResolverContants.pageNumber,
        lmsResolverContants.pageSize,
        '',
        '',
        ''
      )
      .pipe(
        catchError(() => {
          this.notify.error('Problem retrieving data');
          this.router.navigate(['/memberSearch']);
          return of(null);
        })
      );
  }
}
