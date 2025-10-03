import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environmentDev } from '../../../../environments/environment.development';
import { map, Observable, tap } from 'rxjs';
import { FormGroup } from '@angular/forms';
import { ICseGeneralDetails } from '../case details/components/case-details-component/case-details-component';
import {
  IAddCaseForm,
  frmAddContract,
  ICaseRow,
  IListDTO,
  IClientDetails,
  ICourtDetaills,
  IemployeeName,
  IContractRow,
} from '../../../../core/models/requests';
import { enCaseStatus } from '../../../../shared/enums/case-status';
import { Adapter } from '../../../../core/services/adapter/adapter';

@Injectable({
  providedIn: 'root',
})
export class CaseService {
  baseURL = environmentDev.baseURL;
  constructor(private http: HttpClient, private adapter: Adapter) {}

  add(
    createCaseModel: FormGroup<IAddCaseForm>,
    isDraft: boolean
  ): Observable<string> {
    const body = this.adapter.toAddCaseAPI(createCaseModel, isDraft);
    return this.http
      .post(`${this.baseURL}/cases`, body)
      .pipe(map((data) => data as string));
  }

  getClientByNatId(natId: string): Observable<IClientDetails> {
    return this.http
      .get(`${this.baseURL}/clients/national-id/${natId}`)
      .pipe(map((data) => this.adapter.fromClientDetailsAPI(data)));
  }

  getCourtSDetails(): Observable<ICourtDetaills[]> {
    return this.http
      .get<any[]>(`${this.baseURL}/court-types`)
      .pipe(
        map((data) => data.map((ele) => this.adapter.fromCourtDetailsAPI(ele)))
      );
  }

  updateCaseStatus(caseId: string, status: enCaseStatus): Observable<void> {
    return this.http
      .put<any[]>(`${this.baseURL}/cases/${caseId}/status`, { status })
      .pipe(map(() => {}));
  }

  getCasesList(
    courtId: string,
    year: string,
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<IListDTO<ICaseRow>> {
    console.log('the court id', courtId);
    console.log('the year', year);

    return this.http
      .get<any>(
        `${this.baseURL}/cases/by-court-type/${courtId}/year/${year}?pageNumber=${pageNumber}&pageSize=${pageSize}`
      )
      .pipe(
        map((data) =>
          this.adapter.getListDTOAdapter(data, this.adapter.caseListRowAdapter)
        )
      );
  }

  getDraftCasesList(
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<IListDTO<ICaseRow>> {
    return this.http
      .get<any>(
        `${this.baseURL}/cases/list?pageNumber=${pageNumber}&pageSize=${pageSize}&status=${enCaseStatus.Draft}`
      )
      .pipe(
        map((data) =>
          this.adapter.getListDTOAdapter(data, this.adapter.caseListRowAdapter)
        )
      );
  }

  getYearsForCourt(courtId: string): Observable<string[]> {
    return this.http.get<any>(`${this.baseURL}/court-types/${courtId}/years`);
  }

  getCaeDetails(caseId: string): Observable<ICseGeneralDetails> {
    return this.http
      .get<any>(`${this.baseURL}/cases/${caseId}`)
      .pipe(map((res) => this.adapter.fromCaseDetailsAPI(res)));
  }

  addContract(
    caseId: string,
    addContractForm: FormGroup<frmAddContract>
  ): Observable<ICseGeneralDetails> {
    return this.http
      .get<any>(`${this.baseURL}/cases/${caseId}`)
      .pipe(map((res) => this.adapter.fromCaseDetailsAPI(res)));
  }
}
