import { AssetType } from '../_models/assetType';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AssetTypeService {
  baseUrl = environment.apiUrl + 'assettype/';

  constructor(private http: HttpClient) {}

  getCategory(id): Observable<AssetType> {
    return this.http.get<AssetType>(this.baseUrl + id);
  }

  getAssetTypes(): Observable<AssetType[]> {
    return this.http.get<AssetType[]>(this.baseUrl);
  }
}
