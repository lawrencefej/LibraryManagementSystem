import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

import { Author } from '../_models/author';
import { AuthorService } from '../_services/author.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AuthorAssetResolver implements Resolve<Author> {
  constructor(
    private router: Router,
    private authorService: AuthorService,
    private notify: NotificationService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Author> {
    return this.authorService.getAuthor(route.params.id).pipe(
      catchError(() => {
        this.notify.error('Problem retrieving data');
        this.router.navigate(['/authors']);
        return of(null);
      })
    );
  }
}
