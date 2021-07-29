import { Injectable } from '@angular/core';

export interface MinMaxDates {
  maxDate: Date;
  minDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  constructor() {}

  calculateMinMaxDate(): MinMaxDates {
    const currentDay = new Date().getDate();
    const currentMonth = new Date().getMonth();
    const currentYear = new Date().getFullYear();
    const maxDate = new Date(currentYear - 14, currentMonth + 1, currentDay);
    const minDate = new Date(currentYear - 150, currentMonth + 1, currentDay);

    return { maxDate, minDate };
  }
}
