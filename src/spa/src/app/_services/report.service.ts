import { ChartModel } from '../_models/chartModel';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TotalsReport } from '../_models/totalsReport';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  baseUrl = environment.apiUrl + 'reports/';

  constructor(private http: HttpClient) { }

  getAssetDistribution(): Observable<ChartModel> {
    return this.http.get<ChartModel>(this.baseUrl + 'assetDistribution');
  }

  getCategoryDistribution(): Observable<ChartModel> {
    return this.http.get<ChartModel>(this.baseUrl + 'CategoryDistribution');
  }

  getCheckoutByMonth(): Observable<ChartModel> {
    return this.http.get<ChartModel>(this.baseUrl + 'byMonth');
  }

  getReturnsByMonth(): Observable<ChartModel> {
    return this.http.get<ChartModel>(this.baseUrl + 'returnsByMonth');
  }

  getCheckoutByDay(): Observable<ChartModel> {
    return this.http.get<ChartModel>(this.baseUrl + 'byDay');
  }

  getReturnByDay(): Observable<ChartModel> {
    return this.http.get<ChartModel>(this.baseUrl + 'returns');
  }

  getTotals(): Observable<TotalsReport> {
    return this.http.get<TotalsReport>(this.baseUrl + 'totals');
  }

}
