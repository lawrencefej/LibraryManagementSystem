import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {
  baseUrl = environment.apiUrl + 'photo/';

  constructor(private http: HttpClient) {}

  // tslint:disable-next-line: typedef
  changeMemberPhoto(data: any) {
    return this.http.post<Photo>(this.baseUrl + 'userPhoto', data);
  }

  // tslint:disable-next-line: typedef
  changeUserPhoto(data: any) {
    return this.http.post<Photo>(this.baseUrl + 'user-profile-picture', data);
  }

  // tslint:disable-next-line: typedef
  changeAssetPhoto(data: any) {
    return this.http.post<Photo>(this.baseUrl + 'assetPhoto', data);
  }
}
