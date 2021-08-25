import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ReportService } from 'src/app/_services/report.service';
import { DashboardResponse } from 'src/dto/models';

@Component({
  templateUrl: './dashboard-panel.component.html',
  styleUrls: ['./dashboard-panel.component.css']
})
export class DashboardPanelComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  assetTypedChartName = 'Item Type Distribution';
  categoryChartName = 'Category Distribution';
  dailyChartName = 'Daily Activity';
  dashboardData!: DashboardResponse;
  monthlyChartName = 'Monthly Activity';
  totalAuthorsIcon = 'person';
  totalAuthorsName = 'Total Authors';
  totalCheckoutsIcon = 'library_add';
  totalCheckoutsName = 'Total Checkouts';
  totalItemsIcon = 'library_books';
  totalItemsName = 'Total Items';
  totalMembersIcon = 'account_box';
  totalMembersName = 'Total Members';

  constructor(private readonly reportService: ReportService, private readonly route: ActivatedRoute) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  // TODO Handle token refresh and polling data
  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => (this.dashboardData = routeData.initData));
  }
}
