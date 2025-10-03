import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import {
  MatPaginator,
  MatPaginatorModule,
  MatPaginatorIntl,
  PageEvent,
} from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { FormsModule } from '@angular/forms';
import { PageHeaderComponent } from '../../../../../shared/components/page header/page-header-component/page-header-component';
import {
  IListDTO,
  IDraftCaseRow,
  ICaseRow,
} from '../../../../../core/models/requests';
import { ClsHelpers } from '../../../../../shared/util/helpers/cls-helpers';
import { ClsTableUtil } from '../../../../../shared/util/table/cls-table-util';
import { EmptyTable } from '../../../../../shared/components/empty-table/empty-table/empty-table';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CaseService } from '../../services/case-service';
import { enCaseStatus } from '../../../../../shared/enums/case-status';
import { MatMenuModule } from '@angular/material/menu';
import { ToasterService } from '../../../../../core/services/toaster-service';
import { CaseStatus } from '../../cases list/directives/case-status/case-status';

@Component({
  selector: 'app-draft-cases-page',
  imports: [
    MatTableModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSortModule,
    FormsModule,
    EmptyTable,
    DatePipe,
    CaseStatus,
  ],
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  templateUrl: './draft-cases-page.html',
  styleUrl: './draft-cases-page.css',
})
export class DraftCasesPage implements OnInit, AfterViewInit {
  // ViewChild references for pagination and sorting
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // Component properties
  currentPage: number = 0;
  draftCases: IListDTO<ICaseRow> = {} as IListDTO<ICaseRow>;

  // Enhanced data source for table functionality
  draftCasesDataSource = new MatTableDataSource<ICaseRow>([]);

  displayedColumns: string[] = [
    'index',
    'fileNumber',
    'caseNumber',
    'status',
    'courtTypeName',
    'createdDate',
    'employeeName',
    'caseSubject',
    'details',
  ];

  constructor(
    public helper: ClsHelpers,
    private caseService: CaseService,
    private toasterService: ToasterService
  ) {
    this.draftCasesDataSource.data = [
    ];
    this.draftCases.items = this.draftCasesDataSource.data;
  }

  ngOnInit(): void {
    this.loadDraftCases(0, 100);
  }

  ngAfterViewInit(): void {
    // Connect paginator and sort to data source
    this.draftCasesDataSource.paginator = this.paginator;
    this.draftCasesDataSource.sort = this.sort;
  }

  private loadDraftCases(pageIndex: number, pageSize: number): void {
    this.caseService.getDraftCasesList().subscribe((data) => {
      this.draftCases = data;
      this.draftCasesDataSource.data = this.draftCases.items;
    });
  }

  // Filter function for search
  filter(event: any): void {
    const filterValue = event.target.value?.trim() || '';
    this.draftCasesDataSource.filter = filterValue;

    // Reset to first page when filtering
    if (this.draftCasesDataSource.paginator) {
      this.draftCasesDataSource.paginator.firstPage();
    }
  }

  // Handle pagination changes
  handlePage(event: PageEvent): void {
    this.loadDraftCases(event.pageIndex, event.pageSize);
  }

  // Handle sort changes
  handleSort(sortState: Sort): void {
    console.log('Sort changed:', sortState);
  }

  // Action methods for buttons
  approveDraftCase(draftCase: IDraftCaseRow): void {
    this.caseService
      .updateCaseStatus(draftCase.caseId, enCaseStatus.UnderReview)
      .subscribe(() => {
        // Update the status
        this.loadDraftCases(1, 100);
      });
  }

  cancelDraftCase(draftCase: IDraftCaseRow): void {
    if (confirm('هل أنت متأكد من إلغاء هذه القضية')) {
      this.caseService
        .updateCaseStatus(draftCase.caseId, enCaseStatus.Cancelled)
        .subscribe(() => {
          // Remove from data source
          this.draftCasesDataSource.data =
            this.draftCasesDataSource.data.filter(
              (c) => c.caseId != draftCase.caseId
            );
        });
    }
  }

  // Utility methods
  getTotalDraftCases(): number {
    return this.draftCasesDataSource?.data?.length || 0;
  }

  getFilteredDraftCases(): number {
    return this.draftCasesDataSource?.filteredData?.length || 0;
  }

  hasData(): boolean {
    return this.getTotalDraftCases() > 0;
  }

  isLoading(): boolean {
    // Add loading state if needed
    return false;
  }

  // Export functionality (optional)
  exportData(): void {
    const dataToExport = this.draftCasesDataSource.filteredData;
    console.log('Exporting draft cases data:', dataToExport);
    // Implement your export logic here
  }

  // Clear filters
  clearFilter(): void {
    this.draftCasesDataSource.filter = '';
  }

  // Get status color class (optional helper)
  getStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      مسودة: 'text-warning',
      معلقة: 'text-info',
      معتمدة: 'text-success',
      'قيد المراجعة': 'text-primary',
      مرفوضة: 'text-danger',
    };

    return statusMap[status] || 'text-muted';
  }

  // Get status badge class (optional helper)
  getStatusBadgeClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      مسودة: 'badge bg-warning',
      معلقة: 'badge bg-info',
      معتمدة: 'badge bg-success',
      'قيد المراجعة': 'badge bg-primary',
      مرفوضة: 'badge bg-danger',
    };

    return statusMap[status] || 'badge bg-secondary';
  }

  // Bulk actions (optional)
  approveSelectedCases(): void {
    // Implement bulk approval logic
    console.log('Bulk approve selected cases');
  }


  // Search and filter helpers
  searchByStatus(status: string): void {
    this.draftCasesDataSource.filter = status;
  }

  searchByCourt(courtName: string): void {
    this.draftCasesDataSource.filter = courtName;
  }

  resetFilters(): void {
    this.draftCasesDataSource.filter = '';
    if (this.paginator) {
      this.paginator.firstPage();
    }
  }
}
