import { Component, OnInit } from '@angular/core';

import { ChartModel } from 'src/app/_models/chartModel';
import { ReportService } from 'src/app/_services/report.service';
import { TotalsReport } from 'src/app/_models/totalsReport';

@Component({
  selector: 'app-dashboard-panel',
  templateUrl: './dashboard-panel.component.html',
  styleUrls: ['./dashboard-panel.component.css']
})
export class DashboardPanelComponent implements OnInit {
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

  totalMembers: number;
  totalMembersName = 'Total Members';
  totalMembersIcon = 'account_box';
  totalItems: number;
  totalItemsName = 'Total Items';
  totalItemsIcon = 'library_books';
  totalAuthors: number;
  totalAuthorsName = 'Total Authors';
  totalAuthorsIcon = 'person';
  totalCheckouts: number;
  totalCheckoutsName = 'Total Checkouts';
  totalCheckoutsIcon = 'library_add';

  constructor(private reportService: ReportService) {}

  ngOnInit() {
    this.getDailyActivity();
    this.getAssetTypeDistribution();
    this.getCategoryDistribution();
    this.getMonthlyActivity();
    this.getTotals();
  }

  getDailyActivity() {
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

  getMonthlyActivity() {
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

  getAssetTypeDistribution() {
    this.reportService.getAssetDistribution().subscribe((chartModel: ChartModel) => {
      this.assetTypeDistributionData = chartModel.data.map(a => a.count);
      this.assetTypeDistributionLabel = chartModel.data.map(a => a.name);
    });
  }

  getCategoryDistribution() {
    this.reportService.getCategoryDistribution().subscribe((chartModel: ChartModel) => {
      this.categoryDistributionData = chartModel.data.map(a => a.count);
      this.categoryDistributionLabel = chartModel.data.map(a => a.name);
    });
  }

  getTotals() {
    this.reportService.getTotals().subscribe((result: TotalsReport) => {
      this.totalAuthors = result.totalAuthors;
      this.totalCheckouts = result.totalCheckouts;
      this.totalItems = result.totalItems;
      this.totalMembers = result.totalMembers;
    });
  }
}
