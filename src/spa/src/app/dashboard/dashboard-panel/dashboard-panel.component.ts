import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { DashboardResponseViewModel, DashboardService } from '../services/dashboard.service';

@Component({
  templateUrl: './dashboard-panel.component.html',
  styleUrls: ['./dashboard-panel.component.css']
})
export class DashboardPanelComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  assetTypedChartName = 'Item Type Distribution';
  categoryChartName = 'Category Distribution';
  dailyChartName = 'Daily Activity';
  dashboardDataObservable!: Observable<DashboardResponseViewModel>;
  monthlyChartName = 'Monthly Activity';
  totalAuthorsIcon = 'person';
  totalAuthorsName = 'Total Authors';
  totalCheckoutsIcon = 'library_add';
  totalCheckoutsName = 'Total Checkouts';
  totalItemsIcon = 'library_books';
  totalItemsName = 'Total Items';
  totalMembersIcon = 'account_box';
  totalMembersName = 'Total Members';

  constructor(private readonly dashboardService: DashboardService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
    this.dashboardService.stopConnection();
  }

  // TODO Handle token refresh and polling data
  ngOnInit(): void {
    this.dashboardService.startConnection();
    this.dashboardDataObservable = this.dashboardService.getDashboardObservable();
  }
}
