import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { CheckoutForListDto } from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Injectable()
export class CheckoutSharedService {
  private baseUrl = environment.apiUrl + 'checkout';

  constructor(private readonly http: HttpClient) {}

  getCheckoutsForAsset(
    assetId: number,
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
      .get<CheckoutForListDto[]>(`${this.baseUrl}/asset/${assetId}`, {
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

  getCheckoutsForCard(
    cardId: number,
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
      .get<CheckoutForListDto[]>(`${this.baseUrl}/card/${cardId}`, {
        observe: 'response',
        params
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body || [];
          if (response.headers.get('Pagination') != null) {
            // tslint:disable-next-line: no-non-null-assertion
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination')!);
          }
          return paginatedResult;
        })
      );
  }
}
