import { Component, OnInit } from '@angular/core';
import { PageHeaderComponent } from '../../../../../shared/components/page header/page-header-component/page-header-component';
import { TemplateBoxComponent } from '../components/template-box-component/template-box-component';
import { IListDTO, ITemplateBox } from '../../../../../core/models/requests';
import { AddTemplateDialog } from '../dialogs/add-template-dialog/add-template-dialog';
import { DialogRef } from '@angular/cdk/dialog';
import { MatDialog } from '@angular/material/dialog';
import { SectionButton } from '../../../../../shared/components/section-button/section-button';
import { TemplateService } from '../../../../../core/services/template/template-service';
import { EmptyTable } from "../../../../../shared/components/empty-table/empty-table/empty-table";
import { ClsHelpers } from '../../../../../shared/util/helpers/cls-helpers';

@Component({
  selector: 'app-templats-page',
  imports: [PageHeaderComponent, TemplateBoxComponent, SectionButton, EmptyTable],
  templateUrl: './templats-page.html',
  styleUrl: './templats-page.css',
})
export class TemplatesPage implements OnInit {
  templates: ITemplateBox[] = {} as ITemplateBox[];
  
  /**
   *
   */
  constructor(
    private dilogRef: MatDialog,
    private templateService: TemplateService,
    private helper:ClsHelpers
  ) {}
  
  download(tem: ITemplateBox) {
    console.log('template downlading', tem);
        this.templateService
      .download(tem.id, tem.file.filePath)
      .subscribe((blob) => {
        this.helper.download(tem.file.fileName, blob);
      });
  }
  ngOnInit(): void {
    this.templateService.getAll().subscribe((data) => {
      this.templates = data;
    });
  }
  addTemplate() {
    this.dilogRef
      .open(AddTemplateDialog, {
        height: '300px',
        minWidth: '400px',
      })
      .afterClosed()
      .subscribe(() => {
        this.templateService.getAll().subscribe((data) => {
          this.templates = data;
        });
      });
  }
}
