/* tslint:disable */
/* eslint-disable */
import { ChartDto } from './chart-dto';
export interface DashboardResponse {
  categoryDistribution: ChartDto;
  checkoutsByDay: ChartDto;
  checkoutsByMonth: ChartDto;
  returnsByDay: ChartDto;
  returnsByMonth: ChartDto;
  totalAuthors: number;
  totalCards: number;
  totalCheckouts: number;
  totalItems: number;
  typeDistribution: ChartDto;
}
