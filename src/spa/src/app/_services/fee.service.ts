import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FeeService {
  baseUrl = environment.apiUrl + 'fee/';

  constructor(private http: HttpClient) {}

  payFees(libraryCardID: number) {
    return this.http.post<void>(this.baseUrl + libraryCardID, {});
  }
}
