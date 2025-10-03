import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CaseStatus } from '../../../cases list/directives/case-status/case-status';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { ClsTableUtil } from '../../../../../../shared/util/table/cls-table-util';
import { DatePipe, JsonPipe } from '@angular/common';
import { enCaseStatus } from '../../../../../../shared/enums/case-status';
import { FormsModule } from '@angular/forms';
import { CaseService } from '../../../services/case-service';
import { ToasterService } from '../../../../../../core/services/toaster-service';

@Component({
  selector: 'app-case-details-component',
  standalone: true,
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  imports: [MatTableModule, CaseStatus, MatPaginator, MatSortModule, DatePipe, FormsModule, JsonPipe],
  templateUrl: './case-details-component.html',
  styleUrl: './case-details-component.css',
})
export class CaseDetailsComponent implements OnInit {
  case!: ICseGeneralDetails;
  displayedColumns: string[] = [
    'index',
    'name',
    'natId',
    'phone',
    'birthDate',
    'countryName',
    'address',
  ];
  partiesDataSource = new MatTableDataSource<ICasesParties>();
  currentPage = 0;
  status!:enCaseStatus;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  constructor(private route: ActivatedRoute, public helper: ClsHelpers, private caseService:CaseService, private toaster:ToasterService) {}

  ngOnInit(): void {
    this.case = this.route.snapshot.data['caseDetails'] as ICseGeneralDetails;
    this.status = this.case.status;
    this.partiesDataSource.data = this.case.caseParities;
  }
  
  ngAfterViewInit(): void {
    this.partiesDataSource.paginator = this.paginator;
    this.partiesDataSource.sort = this.sort;
  }
  
  filterParties(event: Event): void {
    const value = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.partiesDataSource.filter = value;
  }
  
  handlePage(event: any): void {
    this.currentPage = event.pageIndex;
  }
  update(status: enCaseStatus) {
    if (this.case.status !== status) {
      this.caseService.updateCaseStatus(this.case.caseId, status).subscribe(() => {
        this.toaster.success('تم تغيير الحالة بنجاح')
        this.case.status = status;
      })
    }
  }
}

export interface ICseGeneralDetails {
  caseId: string;
  caseNumber: string;
  caseSubject: string;
  caseParities: ICasesParties[];
  clientRequests: string;
  estimatedDate: Date;
  status: enCaseStatus;
  layerOpinion: string;
  assignedEmployeeName: string;
  employeeName: string; // employee created the case
  courtTypeName: string;
  createdDate: Date;
}

export interface ICasesParties {
  clientId: string;
  name: string;
  natId: string;
  phoneNumber: string;
  countryCode: string;
  address: string;
  birthDate: Date;
}
