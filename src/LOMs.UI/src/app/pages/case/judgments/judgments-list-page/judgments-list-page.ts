import {
  Component,
  Input,
  ViewChild,
  AfterViewInit,
  OnInit,
} from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { IJudgmentRow, IListDTO } from '../../../../../core/models/requests';
import { PageHeaderComponent } from '../../../../../shared/components/page header/page-header-component/page-header-component';
import { DatePipe } from '@angular/common';
import { EmptyTable } from '../../../../../shared/components/empty-table/empty-table/empty-table';
import { ClsHelpers } from '../../../../../shared/util/helpers/cls-helpers';
import { JudgmentService } from '../../../../../core/services/judgments/judgment-service';
import { JudgmentStyleDirective } from '../../../../../shared/directives/judgment-type/judgment-type';
import { JudgmentSubTypeStyleDirective } from '../../../../../shared/directives/judgment-sub-type/judgment-sub-type';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-judgment-table',
  templateUrl: './judgments-list-page.html',
  styleUrl: './judgments-list-page.css',
  imports: [
    MatPaginatorModule,
    PageHeaderComponent,
    MatTableModule,
    DatePipe,
    MatSortModule,
    EmptyTable,
    JudgmentStyleDirective,
    JudgmentSubTypeStyleDirective,
    MatMenuModule,
    RouterLink
  ],
})
export class JudgmentsListPage implements AfterViewInit, OnInit {
  judgments: IJudgmentRow[] = [];
  
  judgmentsList: IListDTO<IJudgmentRow> = {} as IListDTO<IJudgmentRow>;
  
  displayedColumns: string[] = [
    'index',
    'number',
    'type',
    'subType',
    'issueDate',
    'creatorName',
    'createdDate',
    'download',
  ];
  
  dataSource = new MatTableDataSource<IJudgmentRow>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  
  /**
   *
  */
  constructor(
    public helper: ClsHelpers,
    private judgmentService: JudgmentService
  ) {}
  ngOnInit(): void {
    this.judgmentService.getAll().subscribe((res) => {
      this.judgmentsList = res;
      this.dataSource.data = this.judgmentsList.items;
    });
  }
  download(row: IJudgmentRow) {
        this.judgmentService
        .download(row.caseId, row.id, row.file.filePath)
        .subscribe((blob) => this.helper.download(row.file.fileName, blob));
  }

  ngAfterViewInit() {
    this.dataSource.data = this.judgmentsList.items;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value
      .trim()
      .toLowerCase();
    this.dataSource.filter = filterValue;
  }

  getDownloadUrl(row: IJudgmentRow): string {
    return `/files/judgments/${row.number}.pdf`; // Adjust if filePath is available
  }
}
