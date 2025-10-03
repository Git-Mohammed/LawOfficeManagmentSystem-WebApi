import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import {
  MatPaginator,
  MatPaginatorModule,
  MatPaginatorIntl,
  PageEvent,
} from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { FormGroup, FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { AddAttachmentDialog } from '../../dialogs/add-attachment/add-attachment';
import { ClsTableUtil } from '../../../../../../shared/util/table/cls-table-util';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { EmptyTable } from '../../../../../../shared/components/empty-table/empty-table/empty-table';
import { ActivatedRoute } from '@angular/router';
import {
  IAddAttachmetnForm,
  IAttachmentRow,
  IListDTO,
} from '../../../../../../core/models/requests';
import { AttachmentService } from '../../../../../../core/services/attachment/attachment-service';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-case-attachments',
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    FormsModule,
    DatePipe,
    EmptyTable,
    MatMenuModule,
  ],
  providers: [
    {
      provide: MatPaginatorIntl,
      useValue: ClsTableUtil.getArabicPaginatorIntl(),
    },
  ],
  templateUrl: './case-attachments.html',
  styleUrl: './case-attachments.css',
})
export class CaseAttachments implements OnInit, AfterViewInit {
  // ViewChild references for pagination and sorting
  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;

  // Component properties
  currentPage: number = 0;
  attachments: IListDTO<IAttachmentRow> = {} as IListDTO<IAttachmentRow>;
  caseId: string = '';
  // Enhanced data source for table functionality
  attachmentsDataSource = new MatTableDataSource<IAttachmentRow>([]);

  // Updated column definitions to match enhanced template
  displayedColumns: string[] = [
    'index',
    'name',
    'rasiedDate',
    'raisedBy',
    'actions',
  ];

  constructor(
    private dialogof: MatDialog,
    public helper: ClsHelpers,
    private activatedRoute: ActivatedRoute,
    private attachmentService: AttachmentService,
  ) {}

  ngOnInit(): void {
    this.activatedRoute.parent?.params.subscribe((params) => {
      this.caseId = params['caseId'] || '';
      this.loadAttachments();
    });
  }

  ngAfterViewInit(): void {
    // Connect paginator and sort to data source
    this.attachmentsDataSource.paginator = this.paginator;
    this.attachmentsDataSource.sort = this.sort;
  }

  private loadAttachments(pageIndex: number = 1, pageSize: number = 20): void {
    this.attachmentService
      .getAttachments(pageIndex, pageSize, this.caseId)
      .subscribe((data) => {
        this.attachments = data;
        this.attachmentsDataSource.data = this.attachments.items;
      });
  }

  // Filter function for search
  filter(event: any): void {
    const filterValue = event.target.value?.trim() || '';
    this.attachmentsDataSource.filter = filterValue;

    // Reset to first page when filtering
    if (this.attachmentsDataSource.paginator) {
      this.attachmentsDataSource.paginator.firstPage();
    }
  }

  // Handle pagination changes
  handlePage(event: PageEvent): void {
    this.currentPage = event.pageIndex;
  }

  // Handle sort changes
  handleSort(sortState: Sort): void {
    console.log('Sort changed:', sortState);
  }

  // Action methods
  downloadAttachment(attachment: IAttachmentRow): void {
    this.attachmentService.download(
      attachment.id,
      attachment?.file.filePath,
      this.caseId
    ).subscribe((blob) => {
    
      this.helper.download(attachment?.file.fileName, blob);
    });
  }

  deleteAttachment(attachment: IAttachmentRow): void {
    if (confirm(`هل أنت متأكد من حذف المرفق "${attachment.name}"؟`)) {
      // Remove from data source
      const currentData = this.attachmentsDataSource.data;
      const updatedData = currentData.filter(
        (item) => item.id !== attachment.id
      );
      this.attachmentsDataSource.data = updatedData;
    }
  }

  openAddAttachment(): void {
    const dialogRef = this.dialogof.open(AddAttachmentDialog, {
      height: '350px',
      width: '900px',
      disableClose: true,
      data: {
        caseId: this.caseId,
      },
    });

    dialogRef
      .afterClosed()
      .subscribe((result: FormGroup<IAddAttachmetnForm>) => {        
          this.loadAttachments();
      });
  }
}
