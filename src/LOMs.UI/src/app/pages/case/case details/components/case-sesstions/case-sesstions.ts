import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { AddSessionDialog } from '../../dialogs/add-session-dialog/add-session-dialog';
import { IListDTO, ISessionsRow } from '../../../../../../core/models/requests';
import { ClsTableUtil } from '../../../../../../shared/util/table/cls-table-util';
import { EmptyTable } from '../../../../../../shared/components/empty-table/empty-table/empty-table';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { SessionService } from '../../../../../../shared/services/session.service/session-service';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { enDialogMode } from '../../../../../../shared/enums/dialog-mode';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-case-sesstions',
  standalone: true,
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  imports: [MatTableModule, MatPaginator, MatSortModule, EmptyTable, DatePipe, MatMenuModule],
  templateUrl: './case-sesstions.html',
  styleUrl: './case-sesstions.css',
})
export class CaseSessions implements AfterViewInit, OnInit {
  displayedColumns: string[] = [
    'index',
    'date',
    'assignedEmployeeName',
    'createdByEmployeeName',
    'createdDate',
    'actions',
  ];
  sessionsDataSource = new MatTableDataSource<ISessionsRow>();
  sessions: IListDTO<ISessionsRow> = {} as IListDTO<ISessionsRow>;
  currentPage = 0;
  caseId: string = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private dialogof: MatDialog,
    public helper: ClsHelpers,
    private sessionService: SessionService,
    private activatedRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.activatedRoute.parent?.params.subscribe((params) => {
      this.caseId = params['caseId'] || '';
      this.loadSessions();
    });

    this.sessionsDataSource.data = [];
  }

  ngAfterViewInit(): void {
    this.sessionsDataSource.paginator = this.paginator;
    this.sessionsDataSource.sort = this.sort;
  }

  filterSessions(event: Event): void {
    const value = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.sessionsDataSource.filter = value;
  }

  handlePage(event: any): void {
    this.currentPage = event.pageIndex;
  }

  loadSessions() {
    this.sessionService.getSessionsByCaseId(this.caseId).subscribe((data) => {
      this.sessions = data;
      this.sessionsDataSource.data = this.sessions.items;
    });
  }

  openAddSession(): void {
    this.dialogof.open(AddSessionDialog, {
      height: '325x',
      minWidth: '600px',
      data: { caseId: this.caseId, mode: enDialogMode.add },
    }).afterClosed().subscribe(() => {
      this.loadSessions();
    });
  }

  openUpdateSession(sessionId: string): void {
    this.dialogof.open(AddSessionDialog, {
      height: '325x',
      minWidth: '600px',
      data: {
        caseId: this.caseId,
        sessionId: sessionId,
        mode: enDialogMode.update,
      },
    }).afterClosed().subscribe(() => {
      this.loadSessions();
    });;
  }
}
