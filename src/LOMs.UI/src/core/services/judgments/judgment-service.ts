import { Injectable } from '@angular/core';
import { AppService } from '../../../shared/common/app-service';
import { Adapter } from '../adapter/adapter';
import { HttpClient } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { frmAddJudgment, IJudgmentRow } from '../../models/requests';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class JudgmentService extends AppService {
  constructor(private http: HttpClient, private adapter: Adapter) {
    super();
  }
  
  add(judgment: FormGroup<frmAddJudgment>, caseId: string): Observable<string> {
    const formdate = this.adapter.frmAddJudgmentAdapter(judgment);
    return this.http
    .post(`${this.baseURL}/cases/${caseId}/judgments`, formdate)
    .pipe(map((data) => data as string));
  }
  
  getAllByCaseId(caseId: string) {
    return this.http
    .get<any>(`${this.baseURL}/cases/${caseId}/judgments/paged`)
    .pipe(
      map((res) => {
        console.log(res);
        return this.adapter.getListDTOAdapter<IJudgmentRow>(
          res,
          this.adapter.judgmentRowAdapter
        );
      })
    );
  }

  getAll() {
    return this.http
    .get<any>(`${this.baseURL}/judgments/paged`)
    .pipe(
      map((res) => {
        return this.adapter.getListDTOAdapter<IJudgmentRow>(
          res,
          this.adapter.judgmentRowAdapter
        );
      })
    );
  }

  download(caseId: string, judgmentId: string, filePath: string) {
    return this.http.get(`${this.baseURL}/cases/${caseId}/judgments/${judgmentId}/files/download?relativePath=${filePath}`, {responseType: 'blob'})
  }
}
