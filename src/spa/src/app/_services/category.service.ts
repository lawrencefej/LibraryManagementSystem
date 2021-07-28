import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getCategory(id: number): Observable<Category> {
    return this.http.get<Category>(this.baseUrl + 'category/' + id);
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl + 'category/');
  }
}
