import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { AddContractDialog } from '../../dialogs/add-contract/add-contract';
import { ContractType } from '../../directives/contract-type';
import { CaseService } from '../../../services/case-service';
import { ActivatedRoute } from '@angular/router';
import { ClsTableUtil } from '../../../../../../shared/util/table/cls-table-util';
import { EmptyTable } from '../../../../../../shared/components/empty-table/empty-table/empty-table';
import { IContractRow, IListDTO } from '../../../../../../core/models/requests';
import { DatePipe } from '@angular/common';
import { ContractService } from '../../../../../../core/services/contract/contract-service';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { MatMenuItem, MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-case-contract',
  standalone: true,
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  imports: [
    MatTableModule,
    ContractType,
    MatPaginator,
    MatSortModule,
    EmptyTable,
    DatePipe,
    MatMenuModule,
  ],
  templateUrl: './case-contract.html',
  styleUrl: './case-contract.css',
})
export class CaseContract implements OnInit, AfterViewInit {
  displayedColumns: string[] = [
    'index',
    'contractNumber',
    'contractType',
    'totalAmount',
    'issueDate',
    'expirationDate',
    'employeeNameCreatedIt',
    'createdDate',
    'actions',
  ];
  contractsDataSource = new MatTableDataSource<IContractRow>();
  caseId: string = '';
  currentPage = 0;
  contracts: IListDTO<IContractRow> = {} as IListDTO<IContractRow>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private dialogof: MatDialog,
    private activatedRoute: ActivatedRoute,
    private contractService: ContractService,
    public helper: ClsHelpers
  ) {}

  ngOnInit(): void {
    this.activatedRoute.parent?.params.subscribe((params) => {
      this.caseId = params['caseId'] || '';
      this.loadContracts();
    });
  }

  ngAfterViewInit(): void {
    this.contractsDataSource.paginator = this.paginator;
    this.contractsDataSource.sort = this.sort;
  }

  loadContracts(): void {
    this.contractService.getContractsByCaseId(this.caseId).subscribe({
      next: (res) => {
        this.contracts = res;
        this.contractsDataSource.data = this.contracts.items;
      },
    });
  }

  filterContracts(event: Event): void {
    const value = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.contractsDataSource.filter = value;
  }

  handlePage(event: any): void {
    this.currentPage = event.pageIndex;
  }

  openAddContract() {
    this.dialogof
      .open(AddContractDialog, {
        // height: '325x',
        minWidth: '600px',
        data: { caseId: this.caseId },
      })
      .afterClosed()
      .subscribe(() => {
        this.loadContracts();
      });
  }
  download(contract: IContractRow) {
    this.contractService
      .download(contract.id, this.caseId, contract.file.filePath)
      .subscribe((blob) => {
        this.helper.download(contract.file.fileName, blob);
      });
  }

  formatDates(date: Date) {
    if (date) {
      return date.toLocaleString('ar-EG', {
        year: 'numeric', // السنة بأربعة أرقام
        month: 'numeric', // الشهر كاملاً (مثل: أكتوبر)
        day: 'numeric', // اليوم كرقم
      });
    }

    else return '-------'


  }
}
