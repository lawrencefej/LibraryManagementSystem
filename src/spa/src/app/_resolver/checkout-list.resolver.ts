import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

import { Checkout } from '../_models/checkout';
import { CheckoutService } from '../_services/checkout.service';
import { Injectable } from '@angular/core';
import { NotificationService } from '../_services/notification.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class CheckoutListResolver implements Resolve<Checkout[]> {
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private checkoutService: CheckoutService,
    private notify: NotificationService,
    private router: Router
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Checkout[]> {
    return this.checkoutService
      .getPaginatedCheckouts(this.pageNumber, this.pageSize, '', '', '')
      .pipe(
        catchError((error) => {
          this.notify.error('Problem retrieving data');
          this.router.navigate(['/memberSearch']);
          return of(null);
        })
      );
  }
}
