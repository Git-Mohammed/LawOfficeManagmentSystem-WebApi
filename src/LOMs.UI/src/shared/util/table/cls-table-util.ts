import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';

@Injectable({ providedIn: 'root' })
export class ClsTableUtil {
  static getArabicPaginatorIntl(): MatPaginatorIntl {
    const paginatorIntl = new MatPaginatorIntl();
    paginatorIntl.itemsPerPageLabel = 'عدد العناصر في الصفحة';
    paginatorIntl.nextPageLabel = 'الصفحة التالية';
    paginatorIntl.previousPageLabel = 'الصفحة السابقة';
    paginatorIntl.firstPageLabel = 'الصفحة الأولى';
    paginatorIntl.lastPageLabel = 'الصفحة الأخيرة';
    paginatorIntl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) return `0 من ${length}`;
      const start = page * pageSize;
      const end = Math.min(start + pageSize, length);
      return `${start + 1} - ${end} من ${length}`;
    };
    return paginatorIntl;
  }
}
