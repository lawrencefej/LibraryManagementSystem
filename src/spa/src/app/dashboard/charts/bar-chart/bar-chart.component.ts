import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit, OnChanges {
  @Input() chartName!: string;
  @Input() firstData!: ChartDto;
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

  ngOnChanges(changes: SimpleChanges): void {
    this.buildData(changes.firstData.currentValue, changes.secondData.currentValue);
  }

  ngOnInit(): void {
    this.buildData(this.firstData, this.secondData);
  }

  private buildData(firstData: ChartDto, secondData?: ChartDto): void {
    this.barChartLabels = firstData.data.map(a => a.name);
    this.barChartData = [
      {
        data: firstData.data.map(a => a.count),
        label: firstData.label
      }
    ];

    if (secondData) {
      this.barChartData.push({
        data: secondData?.data.map(a => a.count),
        label: secondData?.label
      });
    }
  }
}
