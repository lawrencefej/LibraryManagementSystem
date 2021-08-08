import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LibraryAssetForCreationDto } from 'src/dto/models/library-asset-for-creation-dto';
import { LibraryAssetForDetailedDto } from 'src/dto/models/library-asset-for-detailed-dto';
import { LibraryAssetForListDto } from 'src/dto/models/library-asset-for-list-dto';
import { LibraryAssetForUpdateDto } from 'src/dto/models/library-asset-for-update-dto';
import { environment } from 'src/environments/environment';
import { LibraryAsset } from '../_models/libraryAsset';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class AssetService {
  baseUrl = environment.apiUrl + 'catalog/';

  constructor(private http: HttpClient) {}

  getAssets(): Observable<LibraryAssetForListDto[]> {
    return this.http.get<LibraryAssetForListDto[]>(this.baseUrl);
  }

  getAsset(assetId: number): Observable<LibraryAssetForDetailedDto> {
    return this.http.get<LibraryAssetForDetailedDto>(this.baseUrl + assetId);
  }

  getAssetForAuthor(authorId: number): Observable<LibraryAssetForListDto> {
    return this.http.get<LibraryAssetForListDto>(this.baseUrl + 'author/' + authorId);
  }

  // addAsset(asset: LibraryAsset) {
  //   return this.http.post(this.baseUrl, asset);
  // }

  addAsset(assetForCreation: LibraryAssetForCreationDto): Observable<LibraryAssetForDetailedDto> {
    return this.http.post<LibraryAssetForDetailedDto>(this.baseUrl, assetForCreation);
  }

  searchAsset(name: string): Observable<LibraryAsset[]> {
    return this.http.get<LibraryAsset[]>(this.baseUrl + 'search?SearchString=' + name);
  }

  updateAsset(asset: LibraryAssetForUpdateDto): Observable<void> {
    return this.http.put<void>(this.baseUrl, asset);
  }

  // updateAsset(asset: LibraryAsset) {
  //   return this.http.put(this.baseUrl, asset);
  // }

  getPaginatedAssets(
    page: number,
    itemsPerPage: number,
    orderBy: string,
    sortDirection: string,
    searchString: string
  ): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    const paginatedResult: PaginatedResult<LibraryAssetForListDto[]> = new PaginatedResult<LibraryAssetForListDto[]>();

    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<LibraryAssetForListDto[]>(this.baseUrl, {
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

  deleteAsset(assetId: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + assetId);
  }
}
