import { Injectable } from '@angular/core';
import { environmentDev } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Adapter } from '../../../core/services/adapter/adapter';
import { AppService } from '../../common/app-service';
import {
  IAddSessionForm,
  IListDTO,
  ISessionsRow,
  sessionDetails,
} from '../../../core/models/requests';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class SessionService extends AppService {
  constructor(private http: HttpClient, private adapter: Adapter) {
    super();
  }

  add(
    frmNewSession: FormGroup<IAddSessionForm>,
    caseId: string
  ): Observable<void> {
    const body = this.adapter.createSessionAdapter(frmNewSession);
    return this.http.post<any>(
      `${this.baseURL}/cases/${caseId}/court-sessions`,
      body
    );
  }

  update(updatedSession: FormGroup<IAddSessionForm>, sessionId:string, caseId: string) : Observable<void> {
    console.log('the court session id', sessionId);
    const body = this.adapter.updateSessionAdapter(updatedSession);
    
    return this.http.put<any>(
      `${this.baseURL}/cases/${caseId}/court-sessions/${sessionId}`,
      body
    );
  }
  getSessionsByCaseId(caseId: string): Observable<IListDTO<ISessionsRow>> {
    return this.http
      .get<any>(`${this.baseURL}/cases/${caseId}/court-sessions/paged`)
      .pipe(
        map((data) => {
          console.log('the sessions list', data);
          return this.adapter.getListDTOAdapter(
            data,
            this.adapter.getSessionsByCaseIdAdapter
          )

        }
        )
      );
  }

  getSessionsDetails(
    sessionId: string,
    caseId: string
  ): Observable<sessionDetails> {
    return this.http
      .get<any>(`${this.baseURL}/cases/${caseId}/court-sessions/${sessionId}`)
      .pipe(map((data) => this.adapter.sessionDetailsAdapter(data)));
  }
}
