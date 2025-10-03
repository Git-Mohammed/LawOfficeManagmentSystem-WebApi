import { Injectable } from '@angular/core';
import { AppService } from '../../../shared/common/app-service';
import { Adapter } from '../adapter/adapter';
import { HttpClient } from '@angular/common/http';
import { frmAddTemplate, ITemplateBox } from '../../models/requests';
import { FormGroup } from '@angular/forms';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TemplateService extends AppService {
  constructor(private http: HttpClient, private adapter: Adapter) {
    super();
  }
  
  add(frmAddAttachment: FormGroup<frmAddTemplate>) {
    const formdate = this.adapter.frmAddTemplateAdapter(frmAddAttachment);
    return this.http
      .post(`${this.baseURL}/templates`, formdate)
      .pipe(map((data) => data as string));
  }
  
  getAll(): Observable<ITemplateBox[]> {
    return this.http.get(`${this.baseURL}/templates`).pipe(map(data => this.adapter.templatesList(data)))
  }

  download(id: string, filePath: string) {
    return  this.http
      .get(
        `${this.baseURL}/templates/${id}/files/download?filePath=${filePath}`, {responseType: 'blob'}
      )
  }
}
