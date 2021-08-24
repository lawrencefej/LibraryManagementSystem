import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { DashboardResponse } from 'src/dto/models';
import { DashboardService } from '../services/dashboard.service';

@Injectable()
export class DashboardPanelResolver implements Resolve<DashboardResponse> {
  constructor(private readonly dashboardService: DashboardService) {}

  resolve(): Observable<DashboardResponse> {
    return this.dashboardService.getDashboardData();
  }
}
