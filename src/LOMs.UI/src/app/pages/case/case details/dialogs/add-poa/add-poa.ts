import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { JsonPipe, NgIf } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';
import { DialogHeaderComponent } from '../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';
import { IAddPOAForm } from '../../../../../../core/models/requests';
import { frmApp } from '../../../../../../shared/common/classes/frmApp';
import { HttpContext } from '@angular/common/http';
import { ContractService } from '../../../../../../core/services/contract/contract-service';
import { POAService } from '../../../../../../core/services/poa/poa-service';
import { ToasterService } from '../../../../../../core/services/toaster-service';

@Component({
  selector: 'app-add-poa',
  imports: [
    ReactiveFormsModule,
    NgIf,
    MatDialogModule,
    DialogHeaderComponent,
    JsonPipe,
  ],
  templateUrl: './add-poa.html',
  styleUrl: './add-poa.css',
})
export class AddPoaDialog extends frmApp {
  addPOAForm: FormGroup<IAddPOAForm>;
  constructor(
    private myfb: clsFormsBuilder,
    public dialogRef: MatDialogRef<AddPoaDialog>,
    private poaService: POAService,
    private toastService:ToasterService,
    @Inject(MAT_DIALOG_DATA) private data: any,
  ) {
    super();
    this.addPOAForm = this.myfb.createAddPOAForm();
  }
  submit() {
    this.addPOAForm.markAllAsTouched();

    if (!this.addPOAForm.invalid) {
      this.poaService.add(this.addPOAForm, this.data?.caseId).subscribe({
        next: (response) => {
          this.toastService.success('تم إضافة الوكالة بنجاح');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.log('Error details:', error);
          this.toastService.error('حدث خطا في اضافة الوكالة');
        },
      })
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0] as File;
      // Set the files in the form control
      this.addPOAForm.controls.poaAttachment?.setValue(file);

      // Mark the control as touched and update validity
      this.addPOAForm.controls.poaAttachment?.markAsTouched();
      this.addPOAForm.controls.poaAttachment?.updateValueAndValidity();
    } else {
      // Clear the form control if no files selected
      this.addPOAForm.controls.poaAttachment?.setValue(null);
    }
  }
}
