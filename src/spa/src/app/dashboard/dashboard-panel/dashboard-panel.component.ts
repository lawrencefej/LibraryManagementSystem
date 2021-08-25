import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ChartModel } from 'src/app/_models/chartModel';
import { TotalsReport } from 'src/app/_models/totalsReport';
import { ReportService } from 'src/app/_services/report.service';
import { DashboardResponse } from 'src/dto/models';

@Component({
  templateUrl: './dashboard-panel.component.html',
  styleUrls: ['./dashboard-panel.component.css']
})
export class DashboardPanelComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  dashboardData!: DashboardResponse;

  checkoutsDataByDay: any;
  returnsDataByDay: any;
  dailyActivityLabel: any;
  dailyActivityData: any;
  dailyChartName = 'Daily Activity';
  monthlyActivityData: any;
  monthlyActivityLabel: any;
  checkoutsDataByMonth: any;
  returnsDataByMonth: any;
  monthlyChartName = 'Monthly Activity';
  assetTypeDistributionData: any;
  assetTypeDistributionLabel: any;
  assetTypedChartName = 'Item Type Distribution';
  categoryDistributionData: any;
  categoryDistributionLabel: any;
  categoryChartName = 'Category Distribution';

  totalMembers!: number;
  totalMembersName = 'Total Members';
  totalMembersIcon = 'account_box';
  totalItems!: number;
  totalItemsName = 'Total Items';
  totalItemsIcon = 'library_books';
  totalAuthors!: number;
  totalAuthorsName = 'Total Authors';
  totalAuthorsIcon = 'person';
  totalCheckouts!: number;
  totalCheckoutsName = 'Total Checkouts';
  totalCheckoutsIcon = 'library_add';

  constructor(private readonly reportService: ReportService, private readonly route: ActivatedRoute) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  // TODO Handle token refresh and polling data
  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => (this.dashboardData = routeData.initData));

    console.log(this.dashboardData);

    // this.getDailyActivity();
    // this.getAssetTypeDistribution();
    // this.getCategoryDistribution();
    // this.getMonthlyActivity();
    // this.getTotals();
  }

  getDailyActivity(): void {
    this.reportService.getCheckoutByDay().subscribe((checkouts: ChartModel) => {
      this.reportService.getReturnByDay().subscribe((returns: ChartModel) => {
        this.dailyActivityData = [
          {
            data: checkouts.data.map((a: { count: any }) => a.count),
            label: checkouts.label
          },
          {
            data: returns.data.map((a: { count: any }) => a.count),
            label: returns.label
          }
        ];
      });
      this.dailyActivityLabel = checkouts.data.map((a: { name: any }) => a.name);
    });
  }

  getMonthlyActivity(): void {
    this.reportService.getCheckoutByMonth().subscribe((checkouts: ChartModel) => {
      this.reportService.getReturnsByMonth().subscribe((returns: ChartModel) => {
        this.monthlyActivityData = [
          {
            data: checkouts.data.map((a: { count: any }) => a.count),
            label: checkouts.label
          },
          {
            data: returns.data.map((a: { count: any }) => a.count),
            label: returns.label
          }
        ];
      });
      this.monthlyActivityLabel = checkouts.data.map((a: { name: any }) => a.name);
    });
  }

  getAssetTypeDistribution(): void {
    this.reportService.getAssetDistribution().subscribe((chartModel: ChartModel) => {
      this.assetTypeDistributionData = chartModel.data.map(a => a.count);
      this.assetTypeDistributionLabel = chartModel.data.map(a => a.name);
    });
  }

  getCategoryDistribution(): void {
    this.reportService.getCategoryDistribution().subscribe((chartModel: ChartModel) => {
      this.categoryDistributionData = chartModel.data.map(a => a.count);
      this.categoryDistributionLabel = chartModel.data.map(a => a.name);
    });
  }

  getTotals(): void {
    this.reportService.getTotals().subscribe((result: TotalsReport) => {
      this.totalAuthors = result.totalAuthors;
      this.totalCheckouts = result.totalCheckouts;
      this.totalItems = result.totalItems;
      this.totalMembers = result.totalMembers;
    });
  }
}
