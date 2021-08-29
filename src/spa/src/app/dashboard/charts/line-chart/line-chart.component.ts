import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ChartDataSets, ChartType } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { ChartDto } from 'src/dto/models';

@Component({
  selector: 'lms-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit, OnChanges {
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
  public lineChartType: ChartType = 'line';
  lineChartLabels: Label[] = [];
  lineChartData: ChartDataSets[] = [];

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    this.buildData(changes.firstData.currentValue, changes.secondData.currentValue);
  }

  ngOnInit(): void {
    this.buildData(this.firstData, this.secondData);
  }

  private buildData(firstData: ChartDto, secondData?: ChartDto): void {
    this.lineChartLabels = firstData.data.map(a => a.name);
    this.lineChartData = [
      {
        data: firstData.data.map(a => a.count),
        label: firstData.label
      }
    ];

    if (secondData) {
      this.lineChartData.push({
        data: secondData?.data.map(a => a.count),
        label: secondData?.label
      });
    }
  }
}
