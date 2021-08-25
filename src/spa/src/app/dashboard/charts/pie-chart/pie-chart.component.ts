import { Component, Input, OnInit } from '@angular/core';
import { ChartOptions } from 'chart.js';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {
  // @Input() pieChartData: any[] = [];
  // @Input() pieChartLabels: any[] = [];
  @Input() chartName!: string;
  @Input() chartData: ChartDto = {};

  labels: string[] = [];
  dataset?: number[] = [];

  pieChartOptions: ChartOptions = {
    responsive: true
  };
  colors: any[] = [
    {
      backgroundColor: ['#26547c', '#ff6b6b', '#ffd166']
    }
  ];
  pieChartLegend = true;
  pieChartType = 'pie';

  constructor() {}

  ngOnInit(): void {
    this.dataset = this.chartData.data!.map(a => a.count!);
    this.labels = this.chartData.data!.map(a => a.name!);
  }
}
