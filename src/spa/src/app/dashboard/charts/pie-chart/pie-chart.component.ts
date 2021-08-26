import { Component, Input, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { SingleDataSet } from 'ng2-charts';
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
  pieChartData!: SingleDataSet;

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
  pieChartType: ChartType = 'pie';

  constructor() {}

  ngOnInit(): void {
    this.pieChartData = this.chartData.data.map(a => a.count);
    this.pieChartLabels = this.chartData.data.map(a => a.name);
  }
}
