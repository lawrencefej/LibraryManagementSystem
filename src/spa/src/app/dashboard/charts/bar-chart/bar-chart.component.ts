import { ChartOptions, ChartType } from 'chart.js';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit {
  @Input() barChartData: any[];
  @Input() barChartLabels: any[];
  @Input() chartName: string;

  public barChartOptions: ChartOptions = {
    responsive: true
  };
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;

  constructor() {
  }

  ngOnInit() {}
}
