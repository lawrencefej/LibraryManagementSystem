import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { LibraryAssetForListDto } from 'src/dto/models';
import { LibraryCardForCreationDto } from 'src/dto/models/library-card-for-creation-dto';
import { LibraryCardForDetailedDto } from 'src/dto/models/library-card-for-detailed-dto';
import { LibraryCardForUpdate } from 'src/dto/models/library-card-for-update';
import { LibrarycardForListDto } from 'src/dto/models/librarycard-for-list-dto';
import { PhotoResponseDto } from 'src/dto/models/photo-response-dto';
import { environment } from 'src/environments/environment';

@Injectable()
export class LibraryCardService {
  baseUrl = environment.apiUrl;
  cardUrl = this.baseUrl + 'librarycard';

  constructor(private readonly httpService: HttpClient) {}

  getCardById(cardId: number): Observable<LibraryCardForDetailedDto> {
    return this.httpService.get<LibraryCardForDetailedDto>(`${this.cardUrl}/${cardId}`);
  }

  getCardByCardNumber(cardNumber: string): Observable<LibraryCardForDetailedDto> {
    return this.httpService.get<LibraryCardForDetailedDto>(`${this.cardUrl}/cardnumber/${cardNumber}`);
  }

  updateCard(card: LibraryCardForUpdate): Observable<LibraryCardForDetailedDto> {
    return this.httpService.put<LibraryCardForDetailedDto>(`${this.cardUrl}`, card);
  }

  deleteCard(cardId: number): Observable<void> {
    return this.httpService.delete<void>(`${this.cardUrl}/${cardId}`);
  }

  AddCard(card: LibraryCardForCreationDto): Observable<LibrarycardForListDto> {
    return this.httpService.post<LibrarycardForListDto>(`${this.cardUrl}`, card);
  }

  changeCardPhoto(cardId: number, formData: FormData): Observable<PhotoResponseDto> {
    return this.httpService.post<PhotoResponseDto>(`${this.baseUrl}photo/librarycard/${cardId}`, formData);
  }

  payFees(cardId: number): Observable<void> {
    return this.httpService.post<void>(`${this.baseUrl}fee/${cardId}`, {});
  }

  getCards(
    page: number,
    itemsPerPage: number,
    orderBy: string,
    sortDirection: string,
    searchString: string
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

    return this.httpService
      .get<LibrarycardForListDto[]>(`${this.cardUrl}`, {
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

    return this.httpService
      .get<LibraryAssetForListDto[]>(`${this.baseUrl}catalog`, {
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
