import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import {
  AuthorDto,
  LibraryAssetForCreationDto,
  LibraryAssetForDetailedDto,
  LibraryAssetForListDto,
  LibraryAssetForUpdateDto
} from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Injectable()
export class LibraryAssetService {
  baseUrl = environment.apiUrl;
  assetUrl = this.baseUrl + 'catalog';

  constructor(private readonly http: HttpClient) {}

  getAsset(assetId: number): Observable<LibraryAssetForDetailedDto> {
    return this.http.get<LibraryAssetForDetailedDto>(`${this.assetUrl}/${assetId}`);
  }

  addAsset(assetForCreation: LibraryAssetForCreationDto): Observable<LibraryAssetForDetailedDto> {
    return this.http.post<LibraryAssetForDetailedDto>(`${this.assetUrl}`, assetForCreation);
  }

  updateAsset(asset: LibraryAssetForUpdateDto): Observable<LibraryAssetForDetailedDto> {
    return this.http.put<LibraryAssetForDetailedDto>(`${this.assetUrl}`, asset);
  }

  deleteAsset(assetId: number): Observable<void> {
    return this.http.delete<void>(`${this.assetUrl}/${assetId}`);
  }

  getAuthors(searchString: string): Observable<AuthorDto[]> {
    let params = new HttpParams();
    params = params.append('searchString', searchString);
    return this.http
      .get<AuthorDto[]>(`${this.baseUrl}author`, { observe: 'response', params })
      .pipe(map(response => response.body || []));
  }

  getAssets(
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
      .get<LibraryAssetForListDto[]>(`${this.assetUrl}`, {
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
