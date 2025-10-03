import { Injectable } from '@angular/core';
import { environmentDev } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { map, Observable } from 'rxjs';
import {
  frmAddContract,
  IAddPOAForm,
  IContractRow,
  IListDTO,
  IPOARow,
} from '../../models/requests';
import { Adapter } from '../adapter/adapter';
import { AppService } from '../../../shared/common/app-service';

@Injectable({
  providedIn: 'root',
})
export class POAService extends AppService {
  constructor(private http: HttpClient, private adapter: Adapter) {
    super();
  }

  add(poaFrm: FormGroup<IAddPOAForm>, caseId: string) {
    const formdate = this.adapter.toAddPOAFormAPI(poaFrm);
    return this.http
      .post(`${this.baseURL}/cases/${caseId}/poas`, formdate)
      .pipe(
        map((data) => {
          data as string;
        })
      );
  }

  getCasePOAs(caseId: string): Observable<IListDTO<IPOARow>> {
    return this.http.get<any>(`${this.baseURL}/cases/${caseId}/poas`).pipe(
      map((res) => {
        console.log(res);
        return this.adapter.getListDTOAdapter<IPOARow>(
          res,
          this.adapter.getCasePOARowAdapter
        );
      })
    );
  }
  download(id: string, caseId: string, filePath: string) {
    return  this.http
      .get(
        `${this.baseURL}/cases/${caseId}/poas/${id}/files/download?relativePath=${filePath}`, {responseType: 'blob'}
      )
  }
}
