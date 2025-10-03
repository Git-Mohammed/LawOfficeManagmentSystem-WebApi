import { Injectable } from '@angular/core';
import { environmentDev } from '../../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import {
  IEmployeeDetails,
  IemployeeName,
  IEmployeeRow,
  IListDTO,
  INewClientForm,
  INewEmployee,
} from '../../../../core/models/requests';
import { AppService } from '../../../../shared/common/app-service';
import { Adapter } from '../../../../core/services/adapter/adapter';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService extends AppService {
  constructor(private http: HttpClient, private adapter: Adapter) {
    super();
  }
  add(client: FormGroup<INewEmployee>) {
    return this.http.post(
      `${this.baseURL}/employees`,
      this.adapter.createEmployeeAdapter(client)
    );
  }
  
  update(
    employeeForm: FormGroup<INewEmployee>,
    employeeId: string,
    isActive:boolean
  ): Observable<void> {
    const body = this.adapter.updateEmployeeAdapter(employeeForm);
    body.isActive = isActive
    return this.http.put<any>(
      `${this.baseURL}/employees/${employeeId}`,
      body
    );
  }
  getEmployeeNames(): Observable<IemployeeName[]> {
    return this.http
      .get<any[]>(`${this.baseURL}/employees/names`)
      .pipe(
        map((data) => data.map((ele) => this.adapter.fromEmployeeNamesAPI(ele)))
      );
    }
    getById(employeeId: string): Observable<IEmployeeDetails> {
      return this.http
      .get(`${this.baseURL}/employees/${employeeId}`)
      .pipe(map((data) => this.adapter.employeeDetailsAdapter(data)));
    }

    getAll(pageIndex: number, pageSize: number) : Observable<IEmployeeRow[]> {
          return this.http
      .get(`${this.baseURL}/employees`)
      .pipe(map((data) => this.adapter.getAllEmployees(data)));
    }
}
