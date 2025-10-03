import { Injectable } from '@angular/core';
import { AppService } from '../../../shared/common/app-service';
import { HttpClient } from '@angular/common/http';
import { Adapter } from '../adapter/adapter';
import {
  IAddAttachmetnForm,
  IAttachmentRow,
  IListDTO,
} from '../../models/requests';
import { FormGroup } from '@angular/forms';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AttachmentService extends AppService {
  constructor(private http: HttpClient, private adapter: Adapter) {
    super();
  }

  add(frmAddAttachment: FormGroup<IAddAttachmetnForm>, caseId: string) {
    const formdate = this.adapter.toAddAttachmentFormAPI(frmAddAttachment);
    return this.http
      .post(`${this.baseURL}/cases/${caseId}/attachments`, formdate)
      .pipe(map((data) => data as string));
  }

  getAttachments(
    pageIndex: number,
    pageSize: number,
    caseId: string
  ): Observable<IListDTO<IAttachmentRow>> {
    return this.http
      .get<any>(
        `${this.baseURL}/cases/${caseId}/attachments?pageIndex=${pageIndex}&pageSize=${pageSize}`
      )
      .pipe(
        map((res) => {
          console.log(res);
          return this.adapter.getListDTOAdapter<IAttachmentRow>(
            res,
            this.adapter.getCaseAttachmentRowAdapter
          );
        })
      );
  }

  download(id: string, filePath: string, caseId: string) {
    return this.http
      .get(
        `${this.baseURL}/cases/${caseId}/attachments/${id}/files/download?filePath=${filePath}`, {responseType: 'blob'}
      )
  }
}
