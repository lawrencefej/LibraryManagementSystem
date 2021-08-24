import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DashboardResponse } from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Injectable()
export class DashboardService {
  baseUrl = environment.apiUrl + 'dashboard';

  constructor(private readonly httpService: HttpClient) {}

  getDashboardData(): Observable<DashboardResponse> {
    return this.httpService.get<DashboardResponse>(`${this.baseUrl}`);
  }
}
