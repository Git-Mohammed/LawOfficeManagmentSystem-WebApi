import { Component, Inject } from '@angular/core';
import { frmAddTemplate } from '../../../../../../core/models/requests';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogContent, MatDialogModule } from '@angular/material/dialog';
import { ToasterService } from '../../../../../../core/services/toaster-service';
import { TemplateService } from '../../../../../../core/services/template/template-service';
import { DialogHeaderComponent } from "../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component";
import { NgIf } from '@angular/common';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';


@Component({
  selector: 'app-add-template-dialog',
  imports: [DialogHeaderComponent, MatDialogContent, ReactiveFormsModule,  MatDialogModule],
  templateUrl: './add-template-dialog.html',
  styleUrl: './add-template-dialog.css'
})
export class AddTemplateDialog {
addTemplateForm: FormGroup<frmAddTemplate>;

  constructor(
    private fb: clsFormsBuilder,
    public dialogRef: MatDialogRef<AddTemplateDialog>,
    private templateService: TemplateService,
    private toastService: ToasterService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.addTemplateForm = this.fb.frmAddTemplate();
  }

  submit() {
    this.addTemplateForm.markAllAsTouched();
    if (!this.addTemplateForm.invalid) {
      this.templateService.add(this.addTemplateForm).subscribe({
        next: () => {
          this.toastService.success('تم إضافة القالب بنجاح');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Template add error:', error);
          this.toastService.error('حدث خطأ أثناء إضافة القالب');
        },
      });
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    this.addTemplateForm.controls.file.setValue(file);
    this.addTemplateForm.controls.file.markAsTouched();
    this.addTemplateForm.controls.file.updateValueAndValidity();
  }
}
