import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { CaseService } from '../../services/case-service';
import { IemployeeName } from '../../../../../core/models/requests';
import { EmployeeService } from '../../../employee/services/employee-service';


export const employeeNamesResolver: ResolveFn<Observable<IemployeeName[]>> = (route, state) => {
  return inject(EmployeeService).getEmployeeNames();
};
