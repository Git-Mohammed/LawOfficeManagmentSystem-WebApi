import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'employeeStatus',
})
export class EmployeeStatusPipe implements PipeTransform {
  transform(isActive: boolean): string {
    return isActive ? 'نشط' : 'غير نشط';
  }
}
