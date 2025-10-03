import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { PageHeaderComponent } from '../../../../../../shared/components/page header/page-header-component/page-header-component';
import { ActivatedRoute, RouterLink, RouterLinkActive } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import {
  MatPaginator,
  MatPaginatorModule,
  MatPaginatorIntl,
  PageEvent,
} from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { CaseService } from '../../../services/case-service';
import { FormsModule, NgModel } from '@angular/forms';
import { CaseStatus } from '../../directives/case-status/case-status';
import { DatePipe, NgClass } from '@angular/common';
import {
  ICourtDetaills,
  ICaseRow,
  IListDTO,
} from '../../../../../../core/models/requests';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { ClsTableUtil } from '../../../../../../shared/util/table/cls-table-util';
import { EmptyTable } from '../../../../../../shared/components/empty-table/empty-table/empty-table';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-cases-list-page',
  imports: [
    MatMenuModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    FormsModule,
    RouterLink,
    CaseStatus,
    NgClass,
    EmptyTable,
    DatePipe,
  ],
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  templateUrl: './cases-list-page.html',
  styleUrl: './cases-list-page.css',
})
export class CasesListPage implements OnInit, AfterViewInit {
  // ViewChild references for pagination and sorting
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // Component properties
  activeButton: number = 100;
  currentPage: number = 0;
  courts!: ICourtDetaills[];
  tableRowsData!: ICaseRow[];
  casesList!: IListDTO<ICaseRow>;
  selectedYear!: string;
  selectedCourt!: ICourtDetaills;

  // Enhanced data source for table functionality
  casesDataSource = new MatTableDataSource<ICaseRow>([]);

  displayedColumns: string[] = [
    'index',
    'fileNumber',
    'caseNumber',
    'status',
    'createdDate',
    'employeeName',
    'caseSubject',
    'details',
  ];

  constructor(
    private activatedRoute: ActivatedRoute,
    private caseService: CaseService,
    public helper: ClsHelpers // Added helper for index calculation
  ) {}

  ngOnInit(): void {
    this.initializeComponent();
  }

  ngAfterViewInit(): void {
    // Connect paginator and sort to data source
    this.casesDataSource.paginator = this.paginator;
    this.casesDataSource.sort = this.sort;
  }

  private initializeComponent(): void {
    this.courts = this.activatedRoute.snapshot.data[
      'court'
    ] as ICourtDetaills[];

    if (this.courts && this.courts.length > 0) {
      this.selectedCourt = this.courts[0];
      this.selectedYear = this.selectedCourt?.years[0];
      this.setActiveButton(this.selectedCourt.code);
      this.updateTableData(this.selectedCourt.courtTypeId, this.selectedYear);
    }
  }

  setActiveButton(code: number): void {
    this.activeButton = code;
  }

  onCourtChange(court: ICourtDetaills): void {
    this.selectedCourt = court;
    this.selectedYear = court.years[0]; 
    this.updateTableData(court.courtTypeId, this.selectedYear);
  }

  yearChanged(year: string): void {
    this.selectedYear = year;
  }

  updateTableData(courtId: string, year: string): void {
    this.caseService.getCasesList(courtId, year).subscribe({
      next: (data: IListDTO<ICaseRow>) => {
        console.log('the case list', data);
        this.casesList = data;

        // Update the data source with new data
        if (data?.items && Array.isArray(data.items)) {
          this.casesDataSource.data = data.items;
        } else {
          this.casesDataSource.data = [];
        }

        // Reset to first page when data changes
        if (this.paginator) {
          this.paginator.firstPage();
        }
      },
      error: (error) => {
        console.error('Error loading cases:', error);
        this.casesDataSource.data = [];
      },
    });
  }

  // Filter function for search
  filter(event: any): void {
    const filterValue = event.target.value?.trim() || '';
    this.casesDataSource.filter = filterValue;

    // Reset to first page when filtering
    if (this.casesDataSource.paginator) {
      this.casesDataSource.paginator.firstPage();
    }
  }

  // Handle pagination changes
  handlePage(event: PageEvent): void {
    this.currentPage = event.pageIndex;
    console.log('Page changed:', event);
  }

  // Handle sort changes
  handleSort(sortState: Sort): void {
    console.log('Sort changed:', sortState);
  }

  // Utility methods
  getTotalCases(): number {
    return this.casesDataSource?.data?.length || 0;
  }

  getFilteredCases(): number {
    return this.casesDataSource?.filteredData?.length || 0;
  }

  hasData(): boolean {
    return this.getTotalCases() > 0;
  }

  // Export functionality (optional)
  exportData(): void {
    const dataToExport = this.casesDataSource.filteredData;
    console.log('Exporting data:', dataToExport);
    // Implement your export logic here
  }

  // Refresh data
  refreshData(): void {
    if (this.selectedCourt && this.selectedYear) {
      this.updateTableData(this.selectedCourt.courtTypeId, this.selectedYear);
    }
  }

  // Clear filters
  clearFilter(): void {
    this.casesDataSource.filter = '';
  }
}
