import { Injectable } from '@angular/core';
import { environmentDev } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';

import { FormGroup } from '@angular/forms';
import { map, Observable } from 'rxjs';
import { frmAddContract, IContractRow, IListDTO } from '../../models/requests';
import { Adapter } from '../adapter/adapter';

@Injectable({
  providedIn: 'root',
})
export class ContractService {
  baseURL = environmentDev.baseURL;
  constructor(private http: HttpClient, private adapter: Adapter) {}
  
  add(contractFrm: FormGroup<frmAddContract>, caseId: string) {
    const formdate = this.adapter.toAddContractFormAPI(contractFrm);
    console.log('this is the form data', formdate);
    return this.http
    .post(`${this.baseURL}/cases/${caseId}/contracts`, formdate)
      .pipe(map((data) => data as string));
    }
    
   getContractsByCaseId(caseId: string): Observable<IListDTO<IContractRow>> {
     return this.http.get<any>(`${this.baseURL}/cases/${caseId}/contracts/paged`).pipe(
      map((res) => {
        console.log('the contracts',res);
        return this.adapter.getListDTOAdapter<IContractRow>(
          res,
          this.adapter.getCaseContractRowAdapter
        );
      })
    );
  }
  download(id: string, caseId: string, filePath: string) {
    return  this.http
      .get(
        `${this.baseURL}/cases/${caseId}/contracts/${id}/files/download?relativePath=${filePath}`, {responseType: 'blob'}
      )
  }
}
