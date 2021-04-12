import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiUrl + 'admin/';

  getAdmins() {
    return this.http.get<User[]>(this.baseUrl);
  }

  addUser(user: User) {
    return this.http.post(this.baseUrl, user);
  }

  updateUser(user: User) {
    return this.http.put(this.baseUrl, user);
  }

  deleteUser(userId: number) {
    return this.http.delete(this.baseUrl + userId);
  }
}
