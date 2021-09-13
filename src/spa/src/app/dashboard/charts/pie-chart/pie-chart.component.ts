import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { SingleDataSet } from 'ng2-charts';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit, OnChanges {
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

  ngOnChanges(changes: SimpleChanges): void {
    this.buildData(changes.chartData.currentValue);
  }

  ngOnInit(): void {
    this.buildData(this.chartData);
  }

  private buildData(chartData: ChartDto): void {
    this.pieChartData = chartData.data.map(a => a.count);
    this.pieChartLabels = chartData.data.map(a => a.name);
  }
}
