import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'user/';

  constructor(private http: HttpClient) {}

  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + id);
  }

  // tslint:disable-next-line: typedef
  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + id, user);
  }
}
