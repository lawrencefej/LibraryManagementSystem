import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { ChartsModule } from 'ng2-charts';
import { SharedModule } from '../shared/shared.module';
import { BarChartComponent } from './charts/bar-chart/bar-chart.component';
import { LineChartComponent } from './charts/line-chart/line-chart.component';
import { PieChartComponent } from './charts/pie-chart/pie-chart.component';
import { DashboardPanelComponent } from './dashboard-panel/dashboard-panel.component';
import { DashboardPanelResolver } from './dashboard-panel/dashboard-panel.resolver';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardService } from './services/dashboard.service';
import { TotalCardComponent } from './total-card/total-card.component';

@NgModule({
  declarations: [BarChartComponent, DashboardPanelComponent, LineChartComponent, PieChartComponent, TotalCardComponent],
  imports: [
    ChartsModule,
    SharedModule,
    MatCardModule,
    MatIconModule,
    FlexLayoutModule,
    DashboardRoutingModule,
    CommonModule
  ],
  providers: [DashboardService, DashboardPanelResolver]
})
export class DashboardModule {}
