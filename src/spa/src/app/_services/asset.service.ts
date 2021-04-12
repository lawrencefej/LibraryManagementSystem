import { HttpClient, HttpParams } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { LibraryAsset } from '../_models/libraryAsset';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AssetService {
  baseUrl = environment.apiUrl + 'catalog/';

  constructor(private http: HttpClient) {}

  getAssets(): Observable<LibraryAsset[]> {
    return this.http.get<LibraryAsset[]>(this.baseUrl);
  }

  getAsset(assetId: number): Observable<LibraryAsset> {
    return this.http.get<LibraryAsset>(this.baseUrl + assetId);
  }

  getAssetForAuthor(authorId: number): Observable<LibraryAsset> {
    return this.http.get<LibraryAsset>(this.baseUrl + 'author/' + authorId);
  }

  addAsset(asset: LibraryAsset) {
    return this.http.post(this.baseUrl, asset);
  }

  searchAsset(name: string): Observable<LibraryAsset[]> {
    return this.http.get<LibraryAsset[]>(
      this.baseUrl + 'search?SearchString=' + name
    );
  }

  updateAsset(asset: LibraryAsset) {
    return this.http.put(this.baseUrl, asset);
  }

  getPaginatedAssets(
    page?: number,
    itemsPerPage?: number,
    orderBy?: string,
    sortDirection?: string,
    searchString?: string
  ): Observable<PaginatedResult<LibraryAsset[]>> {
    const paginatedResult: PaginatedResult<
      LibraryAsset[]
    > = new PaginatedResult<LibraryAsset[]>();

    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<LibraryAsset[]>(this.baseUrl + 'pagination', {
        observe: 'response',
        params,
      })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  deleteAsset(assetId: number) {
    return this.http.delete(this.baseUrl + assetId);
  }
}
