import { Component, Input, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit {
  // @Input() barChartData: any[] = [];
  // @Input() barChartLabels: any[] = [];
  @Input() chartName!: string;
  @Input() chartData!: ChartDto;

  labels: string[] = [];
  dataset: number[] = [];

  public barChartOptions: ChartOptions = {
    responsive: true
  };
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;

  constructor() {
    // this.dataset = this.chartData.data!.map(a => a.count!);
    // this.labels = this.chartData.data!.map(a => a.name!);
  }

  ngOnInit(): void {
    console.log(this.chartData);

    this.dataset = this.chartData.data!.map(a => a.count!);
    this.labels = this.chartData.data!.map(a => a.name!);
    console.log(this.dataset);
    console.log(this.labels);
  }
}
