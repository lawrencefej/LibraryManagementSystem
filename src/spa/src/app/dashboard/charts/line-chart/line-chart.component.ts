import { Component, Input } from '@angular/core';
import { Data } from '@angular/router';
import { Color } from 'ng2-charts';
import { ChartModel } from 'src/app/_models/chartModel';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent {
  @Input() lineChartData: any[] = [];
  @Input() lineChartLabels: any[] = [];
  @Input() chartName!: string;

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

  constructor() {
    this.lineChartData = [{ data: [] }];
  }

  chartData!: ChartModel;
  data: Data[] = [];
  chartData2!: ChartModel;
  data2: Data[] = [];
  localLabel: any;
  LocalBarChartLabels: any[] = [];
}
