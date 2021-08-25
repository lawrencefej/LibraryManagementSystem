import { Component, Input, OnInit } from '@angular/core';
import { ChartOptions } from 'chart.js';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {
  @Input() chartName!: string;
  @Input() chartData!: ChartDto;

  pieChartLabels: string[] = [];
  pieChartData?: number[] = [];

  pieChartOptions: ChartOptions = {
    responsive: true,
    legend: {
      position: 'left',
      align: 'center',
      display: true,
      fullWidth: true
    }
  };
  // colors: Color[] = [
  //   {
  //     backgroundColor: ['#26547c', '#ff6b6b', '#ffd166']
  //   }
  // ];
  pieChartLegend = true;
  pieChartType = 'pie';

  constructor() {}

  ngOnInit(): void {
    this.pieChartData = this.chartData.data.map(a => a.count);
    this.pieChartLabels = this.chartData.data.map(a => a.name);
  }
}
