import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

import { Author } from '../_models/author';
import { AuthorService } from '../_services/author.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AuthorListResolver implements Resolve<Author[]> {
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private authorService: AuthorService,
    private notify: NotificationService,
    private router: Router
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Author[]> {
    return this.authorService
      .getPaginatedAuthors(this.pageNumber, this.pageSize, '', '', '')
      .pipe(
        catchError(() => {
          this.notify.error('Problem retrieving data');
          this.router.navigate(['/memberSearch']);
          return of(null);
        })
      );
  }
}
