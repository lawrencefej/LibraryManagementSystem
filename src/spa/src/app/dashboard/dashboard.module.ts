import { BarChartComponent } from './charts/bar-chart/bar-chart.component';
import { ChartsModule } from 'ng2-charts';
import { DashboardPanelComponent } from './dashboard-panel/dashboard-panel.component';
import { LineChartComponent } from './charts/line-chart/line-chart.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { NgModule } from '@angular/core';
import { PieChartComponent } from './charts/pie-chart/pie-chart.component';
import { SharedModule } from '../shared/shared.module';
import { TotalCardComponent } from './total-card/total-card.component';

@NgModule({
  declarations: [
    BarChartComponent,
    DashboardPanelComponent,
    LineChartComponent,
    PieChartComponent,
    TotalCardComponent,
  ],
  imports: [ChartsModule, SharedModule, MatCardModule, MatIconModule],
  exports: [],
})
export class DashboardModule {}
