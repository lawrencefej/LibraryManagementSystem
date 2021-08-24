/* tslint:disable */
/* eslint-disable */
import { ChartDto } from './chart-dto';
export interface DashboardResponse {
  categoryDistribution: ChartDto;
  totalAuthors: number;
  totalCards: number;
  totalCheckouts: number;
  totalItems: number;
  typeDistribution: ChartDto;
}
