// country-name.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

interface ICounty {
  code: string;
  name: string;
}

@Pipe({
  name: 'countryName'
})
export class CountryNamePipe implements PipeTransform {
  private countries: ICounty[] = [
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

  transform(code: string): string {
    const match = this.countries.find(c => c.code === code.toUpperCase());
    return match ? match.name : code;
  }
}
