import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FeeService {
  baseUrl = environment.apiUrl + 'fee/';

  constructor(private http: HttpClient) {}

  payFees(libraryCardID: number): Observable<void> {
    return this.http.post<void>(this.baseUrl + libraryCardID, {});
  }
}
