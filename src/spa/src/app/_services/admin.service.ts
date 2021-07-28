import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl + 'admin/';

  getAdmins(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
  }

  // tslint:disable-next-line: ban-types
  addUser(user: User): Observable<Object> {
    return this.http.post(this.baseUrl, user);
  }

  // tslint:disable-next-line: ban-types
  updateUser(user: User): Observable<Object> {
    return this.http.put(this.baseUrl, user);
  }

  // tslint:disable-next-line: ban-types
  deleteUser(userId: number): Observable<Object> {
    return this.http.delete(this.baseUrl + userId);
  }
}
