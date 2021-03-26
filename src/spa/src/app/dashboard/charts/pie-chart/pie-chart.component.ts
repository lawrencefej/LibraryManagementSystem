import { Component, Input, OnInit } from '@angular/core';

import { ChartOptions } from 'chart.js';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent {
  @Input() pieChartData: any[];
  @Input() pieChartLabels: any[];
  @Input() chartName: string;

  constructor() {}
  public pieChartOptions: ChartOptions = {
    responsive: true
  };
  colors: any[] = [
    {
      backgroundColor: ['#26547c', '#ff6b6b', '#ffd166']
    }
  ];
  public pieChartLegend = true;
  pieChartType = 'pie';

}
