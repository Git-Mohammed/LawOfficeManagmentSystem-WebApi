import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { AddPoaDialog } from '../../dialogs/add-poa/add-poa';
import { ClsTableUtil } from '../../../../../../shared/util/table/cls-table-util';
import { EmptyTable } from '../../../../../../shared/components/empty-table/empty-table/empty-table';
import { ActivatedRoute } from '@angular/router';
import { IAddPOAForm, IListDTO, IPOARow } from '../../../../../../core/models/requests';
import { POAService } from '../../../../../../core/services/poa/poa-service';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-case-poa',
  standalone: true,
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  imports: [MatTableModule, MatPaginator, MatSortModule, EmptyTable, DatePipe, MatMenuModule],
  templateUrl: './case-poa.html',
  styleUrl: './case-poa.css',
})
export class CasePoa implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'publisherName',
    'index',
    'number',
    'creatorName',
    'createdDate',
    'issueDate',
    'actions'
  ];
  POAsDataSource = new MatTableDataSource<IPOARow>();
  poas:IListDTO<IPOARow> = {} as IListDTO<IPOARow>
  currentPage = 0;
  caseId: string = '';
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private dialogof: MatDialog,
    private activatedRoute: ActivatedRoute,
    public helper: ClsHelpers,
    private poaService: POAService
  ) {
    this.POAsDataSource.data = [];
  }
  ngOnInit(): void {
    this.activatedRoute.parent?.params.subscribe((params) => {
      this.caseId = params['caseId'] || '';
      this.loadPOAs();
    });
  }

  ngAfterViewInit(): void {
    this.POAsDataSource.paginator = this.paginator;
    this.POAsDataSource.sort = this.sort;
  }
  
  loadPOAs(): void {
    this.poaService.getCasePOAs(this.caseId).subscribe({
      next: (res) => {
        this.poas = res;
        this.POAsDataSource.data = this.poas.items;
      },
    });
  }
  
  filterPOAs(event: Event): void {
    const value = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.POAsDataSource.filter = value;
  }
  
  handlePage(event: any): void {
    this.currentPage = event.pageIndex;
  }
  
  openAddPOA(): void {
    this.dialogof.open(AddPoaDialog, {
      // height: '325x',
      minWidth: '600px',
      data: { caseId: this.caseId },
    }).afterClosed().subscribe((res:FormGroup<IAddPOAForm>) => {
      if (res) {
        this.loadPOAs()
      }
    });
  }
  download(poa: IPOARow) {
    console.log('dwoloaindg the poa', poa);
    this.poaService.download(poa.id, poa.caseId, poa.file.filePath).subscribe((blob) => this.helper.download(poa.file.fileName, blob))
  }
}
