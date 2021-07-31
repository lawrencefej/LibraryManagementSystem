import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LibraryCardForDetailedDto } from 'src/dto/models';
import { environment } from 'src/environments/environment';
import { LibraryCardAdvancedSearchViewModel } from '../models/library-card-advanced-search-view-model';

@Injectable()
export class SearchService {
  baseUrl = environment.apiUrl;
  cardUrl = this.baseUrl + 'librarycard';

  constructor(private readonly httpService: HttpClient) {}

  getCardByCardNumber(cardNumber: string): Observable<LibraryCardForDetailedDto> {
    return this.httpService.get<LibraryCardForDetailedDto>(`${this.cardUrl}/cardnumber/${cardNumber}`);
  }

  advancedCardSearch(card: LibraryCardAdvancedSearchViewModel): Observable<LibraryCardForDetailedDto[]> {
    let params = new HttpParams();

    params = params.set('firstName', card.firstName);
    params = params.set('lastName', card.lastName);
    params = params.set('email', card.email);
    params = params.set('phoneNumber', card.phoneNumber);
    params = params.set('zipCode', card.zipcode);
    params = params.set('dataOfBirth', card.dateOfBirth);

    return this.httpService.get<LibraryCardForDetailedDto[]>(`${this.cardUrl}/advancedSearch`, { params });
  }
}
