import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

import { Checkout } from '../_models/checkout';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '../_models/pagination';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { CheckoutForDetailedDto } from 'src/dto/models/checkout-for-detailed-dto';
import { CheckoutForListDto } from 'src/dto/models/checkout-for-list-dto';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private baseUrl = environment.apiUrl + 'checkout/';
  private checkout = new Subject<Checkout>();

  constructor(private http: HttpClient) {}

  getCheckout(checkoutId: number): Observable<CheckoutForDetailedDto> {
    return this.http.get<CheckoutForDetailedDto>(this.baseUrl + checkoutId);
  }

  getCheckouts(): Observable<CheckoutForListDto[]> {
    return this.http.get<CheckoutForListDto[]>(this.baseUrl);
  }

  getCheckoutsForCard(userId: number): Observable<CheckoutForListDto[]> {
    return this.http.get<CheckoutForListDto[]>(this.baseUrl + 'card/' + userId);
  }

  searchCheckouts(searchString: string): Observable<CheckoutForListDto[]> {
    return this.http.get<CheckoutForListDto[]>(this.baseUrl + '/search?SearchString=' + searchString);
  }

  returnAsset(id: number) {
    return this.http.put<void>(this.baseUrl + id, {});
  }

  getCheckoutsForAsset(assetId: number): Observable<CheckoutForListDto[]> {
    return this.http.get<CheckoutForListDto[]>(this.baseUrl + 'asset/' + assetId);
  }

  checkoutAsset(checkout: Checkout) {
    return this.http.post(this.baseUrl, checkout);
  }

  checkoutAssets(checkouts: Checkout[]) {
    return this.http.post(this.baseUrl, checkouts);
  }

  getNewCheckout(): Observable<Checkout> {
    return this.checkout.asObservable();
  }

  sendNewCheckout(checkout: Checkout) {
    this.checkout.next(checkout);
  }

  getPaginatedCheckouts(
    page?: number,
    itemsPerPage?: number,
    orderBy?: string,
    sortDirection?: string,
    searchString?: string
  ): Observable<PaginatedResult<CheckoutForListDto[]>> {
    const paginatedResult: PaginatedResult<CheckoutForListDto[]> = new PaginatedResult<CheckoutForListDto[]>();

    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<CheckoutForListDto[]>(this.baseUrl, {
        observe: 'response',
        params
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }
}
