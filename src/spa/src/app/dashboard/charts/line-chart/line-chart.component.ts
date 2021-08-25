import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {
  @Input() chartName!: string;
  @Input() firstData!: ChartDto;
  @Input() secondData?: ChartDto;

  lineChartOptions: any = {
    responsive: true,
    maintainAspectRatio: false
  };
  public lineChartColors: Color[] = [
    {
      // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    },
    {
      // red
      backgroundColor: 'rgba(255,0,0,0.3)',
      borderColor: 'red',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend = true;
  public lineChartType = 'line';
  lineChartLabels: Label[] = [];
  lineChartData: ChartDataSets[] = [];

  constructor() {}

  ngOnInit(): void {
    this.lineChartLabels = this.firstData.data.map(a => a.name);
    this.lineChartData = [
      {
        data: this.firstData.data.map(a => a.count),
        label: this.firstData.label
      }
    ];

    if (this.secondData) {
      this.lineChartData.push({
        data: this.secondData?.data.map(a => a.count),
        label: this.secondData?.label
      });
    }
  }
}
