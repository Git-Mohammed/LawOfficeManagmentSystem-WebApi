import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { PageHeaderComponent } from '../../../../../shared/components/page header/page-header-component/page-header-component';
import { MatDialog } from '@angular/material/dialog';
import { AddEmployeeComponent } from '../../dialogs/add-employee-component';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import {
  MatPaginator,
  MatPaginatorIntl,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { Sort, MatSortModule, MatSort } from '@angular/material/sort';
import {
  IemployeeName,
  IEmployeeRow,
  IListDTO,
} from '../../../../../core/models/requests';
import { ClsHelpers } from '../../../../../shared/util/helpers/cls-helpers';
import { ClsTableUtil } from '../../../../../shared/util/table/cls-table-util';
import { EmptyTable } from '../../../../../shared/components/empty-table/empty-table/empty-table';
import { ScrollStrategyOptions } from '@angular/cdk/overlay';
import { enDialogMode } from '../../../../../shared/enums/dialog-mode';
import { ChangePasswordDialog } from '../../dialogs/change-password/change-password-dialog/change-password-dialog';
import { SectionButton } from '../../../../../shared/components/section-button/section-button';
import { RoleStyleDirective } from '../../directives/role-style/role-style';
import { MatMenuModule } from '@angular/material/menu';
import { EmployeeStatusPipe } from '../../pipes/IsActive/IsActive';
import { EmployeeStatusStyleDirective } from '../../directives/employee-status/employee-status';
import { EmployeeService } from '../../services/employee-service';
import { CountryNamePipe } from '../../../../../shared/pipes/country-name-pipe';
import { SessionManagement } from '../../../../../core/services/session/session-management';
import { enRole } from '../../../../../shared/enums/roles';

@Component({
  selector: 'app-mange-employee-page',
  imports: [
    RoleStyleDirective,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    EmptyTable,
    SectionButton,
    MatMenuModule,
    EmployeeStatusPipe,
    EmployeeStatusStyleDirective,
    CountryNamePipe
  ],
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  templateUrl: './mange-employee-page.html',
  styleUrl: './mange-employee-page.css',
})
export class MangeEmployeePage implements OnInit {

  employees: IListDTO<IEmployeeRow> = {} as IListDTO<IEmployeeRow>;
  sortedData: IEmployeeRow[] = [];
  currentPage: number = 0;

  element: IEmployeeRow[] = [];
  employeesDataSource = new MatTableDataSource<IEmployeeRow>(this.element);
  session = inject(SessionManagement)
  deactive = signal((this.session.getSession() ? this.session.getSession()?.role == enRole.GeneralManager : false))

  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;

  displayedColumns: string[] = [
    'positon',
    'name',
    'natId',
    'email',
    'role',
    'countryCode',
    'IsActive',
    'actions',
  ];
  constructor(
    private dialogof: MatDialog,
    public helper: ClsHelpers,
    private employeeService: EmployeeService
  ) {}
  ngOnInit(): void {
    this.loadData(1, 20);
  }

  ngAfterViewInit() {
    this.employeesDataSource.paginator = this.paginator;
    this.employeesDataSource.sort = this.sort;
  }

  handelPage($event: PageEvent) {
    this.loadData($event.pageIndex, $event.pageSize);
  }
  loadData(pageIndex: number, pageSize: number) {
    this.employeeService.getAll(pageIndex, pageSize).subscribe((data) => {
      this.employeesDataSource.data = data;
    });
  }
  filter($event: any) {
    this.employeesDataSource.filter = $event.target.value;
  }
  addEmployee() {
    this.dialogof.open(AddEmployeeComponent, {
      // height: '500px',
      minWidth: '900px',
      data: { mode: enDialogMode.add },
    }).afterClosed().subscribe(() => {
      this.loadData(1, 100);
    });
  }

  update(employeeId: string) {
    this.dialogof.open(AddEmployeeComponent, {
      height: '500px',
      minWidth: '900px',
      data: { mode: enDialogMode.update, employeeId: employeeId },
    }).afterClosed().subscribe(() => {
      this.loadData(1, 100);
    });;
  }
}
