import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { LibraryCardForCreationDto } from 'src/dto/models/library-card-for-creation-dto';
import { LibraryCardForDetailedDto } from 'src/dto/models/library-card-for-detailed-dto';
import { LibraryCardForUpdate } from 'src/dto/models/library-card-for-update';
import { LibrarycardForListDto } from 'src/dto/models/librarycard-for-list-dto';
import { environment } from 'src/environments/environment';

@Injectable()
export class LibraryCardService {
  baseUrl = environment.apiUrl + 'librarycard/';

  constructor(private http: HttpClient) {}

  getCardById(libraryCardId: number): Observable<LibraryCardForDetailedDto> {
    return this.http.get<LibraryCardForDetailedDto>(this.baseUrl + libraryCardId);
  }

  getCardByCardNumber(cardNumber: string): Observable<LibraryCardForDetailedDto> {
    return this.http.get<LibraryCardForDetailedDto>(this.baseUrl + 'cardnumber/' + cardNumber);
  }

  // advancedCardSearch(member: User): Observable<User[]> {
  //   let params = new HttpParams();

  //   params = params.set('firstName', member.firstName);
  //   params = params.set('lastName', member.lastName);
  //   params = params.set('email', member.email);
  //   return this.http.get<User[]>(this.baseUrl + 'advancedSearch', { params });
  // }

  updateCard(card: LibraryCardForUpdate) {
    return this.http.put(this.baseUrl, card);
  }

  deleteCard(cardId: number) {
    return this.http.delete(this.baseUrl + cardId);
  }

  AddCard(card: LibraryCardForCreationDto) {
    return this.http.post(this.baseUrl, card);
  }

  getCards(
    page?: number,
    itemsPerPage?: number,
    orderBy?: string,
    sortDirection?: string,
    searchString?: string
  ): Observable<PaginatedResult<LibrarycardForListDto[]>> {
    const paginatedResult: PaginatedResult<LibrarycardForListDto[]> = new PaginatedResult<LibrarycardForListDto[]>();

    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pagenumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<LibrarycardForListDto[]>(this.baseUrl + 'pagination', {
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
