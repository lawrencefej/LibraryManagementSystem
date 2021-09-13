import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { checkoutFilters } from 'src/app/shared/constants/checkout.constant';
import { PaginatedResult } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { CheckoutForListDto } from 'src/dto/models';

@Injectable()
export class CheckoutListResolver implements Resolve<Observable<PaginatedResult<CheckoutForListDto[]>>> {
  constructor(private checkoutService: CheckoutService) {}
  resolve(): Observable<PaginatedResult<CheckoutForListDto[]>> {
    return this.checkoutService.getPaginatedCheckouts(
      lmsResolverContants.pageNumber,
      lmsResolverContants.pageSize,
      '',
      '',
      checkoutFilters[0]
    );
  }
}
