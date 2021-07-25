import { Injectable } from '@angular/core';
import { Router, Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibrarycardForListDto } from 'src/dto/models';
import { LibraryCardService } from '../services/library-card.service';

@Injectable()
export class LibraryCardListResolver implements Resolve<LibrarycardForListDto[]> {
  constructor(
    private readonly cardService: LibraryCardService,
    private readonly router: Router,
    private readonly notify: NotificationService
  ) {}
  resolve(): Observable<LibrarycardForListDto[]> {
    return this.cardService.getCards(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '').pipe(
      catchError(() => {
        this.notify.error('Problem retrieving data');
        this.router.navigate(['/library-card/home']);
        return of(null);
      })
    );
  }
}
