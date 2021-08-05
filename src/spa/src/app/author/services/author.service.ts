import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { AuthorDto, LibraryAssetForListDto } from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthorService {
  baseUrl = environment.apiUrl;
  authorUrl = `${this.baseUrl}author`;

  constructor(private readonly httpService: HttpClient) {}

  getAuthor(authorId: number): Observable<AuthorDto> {
    return this.httpService.get<AuthorDto>(`${this.authorUrl}/${authorId}`);
  }

  updateAuthor(author: AuthorDto): Observable<AuthorDto> {
    return this.httpService.put<AuthorDto>(`${this.authorUrl}`, author);
  }

  addAuthor(author: AuthorDto): Observable<AuthorDto> {
    return this.httpService.post<AuthorDto>(`${this.authorUrl}`, author);
  }

  deleteAsset(authorId: number): Observable<void> {
    return this.httpService.delete<void>(`${this.authorUrl}/${authorId}`);
  }

  getAuthors(
    page: number,
    itemsPerPage: number,
    orderBy: string,
    sortDirection: string,
    searchString: string
  ): Observable<PaginatedResult<AuthorDto[]>> {
    const paginatedResult: PaginatedResult<AuthorDto[]> = new PaginatedResult<AuthorDto[]>();

    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.httpService
      .get<AuthorDto[]>(`${this.authorUrl}`, {
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

  getAssetsForAuthors(
    authorId: number,
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

    return this.httpService
      .get<LibraryAssetForListDto[]>(`${this.authorUrl}/${authorId}`, {
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
