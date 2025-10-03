import { Component, Inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule, DatePipe, JsonPipe, NgIf } from '@angular/common';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { ToasterService } from '../../../../../../core/services/toaster-service';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';
import { frmAddContract } from '../../../../../../core/models/requests';
import { ContractService } from '../../../../../../core/services/contract/contract-service';
import { DialogHeaderComponent } from '../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';
import { datesValidator } from '../../../../../../shared/validators/Date/dates-validators';
import { enContractType } from '../../../../../../shared/enums/contract-types';

@Component({
  selector: 'app-add-contract',
  imports: [
    JsonPipe,
    ReactiveFormsModule,
    MatDialogModule,
    DialogHeaderComponent,
    DatePipe,
    CommonModule
  ],
  templateUrl: './add-contract.html',
  styleUrl: './add-contract.css',
})
export class AddContractDialog {
  contractForm: FormGroup<frmAddContract>;
  fixedTerm: enContractType = enContractType.FixedTerm;
  constructor(
    private formBuilder: clsFormsBuilder,
    private toastService: ToasterService,
    private contractService: ContractService,
    @Inject(MAT_DIALOG_DATA) private data: any,
    public dialogRef: MatDialogRef<AddContractDialog>,
    public dateValidator: datesValidator
  ) {
    this.contractForm = this.formBuilder.createAddContractForm();
  }

  submit() {
    this.contractForm.markAllAsTouched();

    
    if (this.contractForm.invalid) {
      return;
    } else {
      this.contractService.add(this.contractForm, this.data.caseId).subscribe({
        next: (response) => {
          this.toastService.success('تم إضافة العقد بنجاح');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.log('Error details:', error);
          this.toastService.error('حدث خطا في اضافة العقد');
        },
      });
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      const file = input.files[0];
      // Set the files in the form control
      this.contractForm.controls.contractAttachment?.setValue(file);

      // Mark the control as touched and update validity
      this.contractForm.controls.contractAttachment?.markAsTouched();
      this.contractForm.controls.contractAttachment?.updateValueAndValidity();
    } else {
      // Clear the form control if no files selected
      this.contractForm.controls.contractAttachment?.setValue(null);
    }
  }
}
