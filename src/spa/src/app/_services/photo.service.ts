import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  baseUrl = environment.apiUrl + 'photo/';

  constructor(private http: HttpClient) {}

  changeMemberPhoto(data: any) {
    return this.http.post(this.baseUrl + 'userPhoto', data);
  }

  changeUserPhoto(data: any) {
    return this.http.post(this.baseUrl + 'user-profile-picture', data);
  }

  changeAssetPhoto(data: any) {
    return this.http.post(this.baseUrl + 'assetPhoto', data);
  }
}
