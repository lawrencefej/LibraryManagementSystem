import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit {
  @Input() chartName!: string;
  @Input() chartData!: ChartDto;
  @Input() secondData?: ChartDto;

  labels: string[] = [];
  dataset: number[] = [];

  barChartOptions: ChartOptions = {
    responsive: true
  };
  barChartType: ChartType = 'bar';
  barChartLegend = true;
  barChartLabels: Label[] = [];
  barChartData: ChartDataSets[] = [];

  constructor() {}

  ngOnInit(): void {
    this.barChartLabels = this.chartData.data.map(a => a.name);
    this.barChartData = [
      {
        data: this.chartData.data.map(a => a.count),
        label: this.chartData.label
      }
    ];

    if (this.secondData) {
      this.barChartData.push({
        data: this.secondData?.data.map(a => a.count),
        label: this.secondData?.label
      });
    }
  }
}
