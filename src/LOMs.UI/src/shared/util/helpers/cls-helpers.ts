import { Injectable } from '@angular/core';
import { Country } from '@wlucha/ng-country-select';
import { ICounty } from '../../../core/models/requests';

@Injectable({ providedIn: 'root' })
export class ClsHelpers {
  getCurrentRowNumber(currentRow: number, pageIndex: number, pageSize: number) {
    return currentRow + 1 + pageIndex * pageSize;
  }

  formatDateForInput(dateString: Date): string {
    return dateString.toISOString().split('T')[0];
  }

  loadCountries(): ICounty[] {
    return [
      { code: 'SA', name: 'السعودية' },
      { code: 'EG', name: 'مصر' },
      { code: 'AE', name: 'الإمارات' },
      { code: 'JO', name: 'الأردن' },
      { code: 'IQ', name: 'العراق' },
      { code: 'MA', name: 'المغرب' },
      { code: 'DZ', name: 'الجزائر' },
      { code: 'TN', name: 'تونس' },
      { code: 'LB', name: 'لبنان' },
      { code: 'SY', name: 'سوريا' },
      { code: 'YE', name: 'اليمن' },
      { code: 'OM', name: 'عُمان' },
      { code: 'KW', name: 'الكويت' },
      { code: 'QA', name: 'قطر' },
      { code: 'BH', name: 'البحرين' },
    ];
  }

  download(fileName:string, blob:Blob) {
          // const blob = new Blob([response], {type : response.type});
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download =fileName;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
  }
}
