import { HttpClient, HttpParams } from '@angular/common/http';

import { Author } from '../_models/author';
import { Checkout } from '../_models/checkout';
import { Injectable } from '@angular/core';
import { LibraryAsset } from '../_models/libraryAsset';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthorService {
  baseUrl = environment.apiUrl + 'author/';

  constructor(private http: HttpClient) {}

  getAuthor(id: string): Observable<Author> {
    return this.http.get<Author>(this.baseUrl + id);
  }

  getAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.baseUrl);
  }

  searchAuthors(searchString: string): Observable<Author[]> {
    return this.http.get<Author[]>(
      this.baseUrl + 'search?SearchString=' + searchString
    );
  }

  updateAuthors(author: Author) {
    return this.http.put(this.baseUrl, author);
  }

  getAssetForAuthor(authorId: number): Observable<LibraryAsset[]> {
    return this.http.get<LibraryAsset[]>(
      environment.apiUrl + 'catalog/author/' + authorId
    );
  }

  addAuthor(author: Author) {
    return this.http.post(this.baseUrl, author);
  }

  getPaginatedAuthors(
    page?: number,
    itemsPerPage?: number,
    orderBy?: string,
    sortDirection?: string,
    searchString?: string
  ): Observable<PaginatedResult<Author[]>> {
    const paginatedResult: PaginatedResult<Author[]> = new PaginatedResult<
      Author[]
    >();

    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<Checkout[]>(this.baseUrl + 'pagination', {
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
}
