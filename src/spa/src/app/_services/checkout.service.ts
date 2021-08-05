import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BasketForCheckoutDto, CheckoutForCheckInDto } from 'src/dto/models';
import { CheckoutForDetailedDto } from 'src/dto/models/checkout-for-detailed-dto';
import { CheckoutForListDto } from 'src/dto/models/checkout-for-list-dto';
import { environment } from 'src/environments/environment';
import { Checkout } from '../_models/checkout';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private baseUrl = environment.apiUrl + 'checkout/';

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

  returnAsset(id: number): Observable<void> {
    return this.http.put<void>(this.baseUrl + id, {});
  }

  checkInAsset(checkout: CheckoutForCheckInDto): Observable<void> {
    return this.http.put<void>(this.baseUrl, checkout);
  }

  getCheckoutsForAsset(assetId: number): Observable<CheckoutForListDto[]> {
    return this.http.get<CheckoutForListDto[]>(this.baseUrl + 'asset/' + assetId);
  }

  checkoutBasket(basket: BasketForCheckoutDto): Observable<void> {
    return this.http.post<void>(this.baseUrl, basket);
  }

  checkoutAsset(checkout: Checkout): Observable<void> {
    return this.http.post<void>(this.baseUrl, checkout);
  }

  checkoutAssets(checkouts: Checkout[]): Observable<void> {
    return this.http.post<void>(this.baseUrl, checkouts);
  }

  getPaginatedCheckouts(
    page: number,
    itemsPerPage: number,
    orderBy: string,
    sortDirection: string,
    searchString: string
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
          paginatedResult.result = response.body || [];
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')!);
          }
          return paginatedResult;
        })
      );
  }
}
