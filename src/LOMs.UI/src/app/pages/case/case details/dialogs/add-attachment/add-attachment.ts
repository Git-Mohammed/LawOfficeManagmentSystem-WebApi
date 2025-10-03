import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';
import {
  MAT_DIALOG_DATA,
  MatDialogActions,
  MatDialogContent,
  MatDialogRef,
} from '@angular/material/dialog';
import { DialogHeaderComponent } from '../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';
import { IAddAttachmetnForm } from '../../../../../../core/models/requests';
import { AttachmentService } from '../../../../../../core/services/attachment/attachment-service';
import { ToasterService } from '../../../../../../core/services/toaster-service';

@Component({
  selector: 'app-add-attachment',
  imports: [
    ReactiveFormsModule,
    NgIf,
    MatDialogActions,
    DialogHeaderComponent,
    MatDialogContent,
  ],
  templateUrl: './add-attachment.html',
  styleUrl: './add-attachment.css',
})
export class AddAttachmentDialog {
  addAttachmentForm: FormGroup<IAddAttachmetnForm>;

  constructor(
    private myfb: clsFormsBuilder,
    public dialogRef: MatDialogRef<AddAttachmentDialog>,
    private attachmentService: AttachmentService,
    private toastService: ToasterService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.addAttachmentForm = this.myfb.createAddAttachmentForm();
  }
  submit() {
    if (!this.addAttachmentForm.invalid) {
      this.attachmentService
        .add(this.addAttachmentForm, this.data.caseId)
        .subscribe({
          next: (response) => {
            this.toastService.success('تم إضافة المرفق بنجاح');
            this.dialogRef.close(true);
          },
          error: (error) => {
            console.log('Error details:', error);
            this.toastService.error('حدث خطا في اضافة المرفق');
          },
        });
    }
  }

  onFileChange(event: Event) {
    console.log('enetered the on file change evenet in the attachment');
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      // Set the files in the form control
      this.addAttachmentForm.controls.attachmentFile?.setValue(file);
      // Mark the control as touched and update validity
      this.addAttachmentForm.controls.attachmentFile?.markAsTouched();
      this.addAttachmentForm.controls.attachmentFile?.updateValueAndValidity();
    } else {
      // Clear the form control if no files selected
      this.addAttachmentForm.controls.attachmentFile?.setValue(null);
    }
  }
}
