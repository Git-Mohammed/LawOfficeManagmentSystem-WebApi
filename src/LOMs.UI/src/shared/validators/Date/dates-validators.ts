import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class datesValidator {
  isDate1LessThanDate2(date1: string, date2: string) {
    const convertedDate1: Date = new Date(date1);
    const convertedDate2: Date = new Date(date2);
    console.log('first date', convertedDate1);
    console.log('second date', convertedDate2);

    const isLess = convertedDate1.getTime() < convertedDate2.getTime();
    console.log('the combarsion results', isLess);
    return isLess;
  }

  private createComparable(date: Date) {
    return date.setHours(0, 0, 0);
  }
}
