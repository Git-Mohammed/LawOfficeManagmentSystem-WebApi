import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { JsonPipe, NgIf } from '@angular/common';
import { ToasterService } from '../../../../../../core/services/toaster-service';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';
import { frmAddJudgment } from '../../../../../../core/models/requests';
import { DialogHeaderComponent } from '../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';
import {
  enJudgmentSubType,
  enJudgmentType,
} from '../../../../../../shared/enums/enums';
import { JudgmentService } from '../../../../../../core/services/judgments/judgment-service';

@Component({
  selector: 'app-add-judgment',
  imports: [
    JsonPipe,
    ReactiveFormsModule,
    MatDialogModule,
    DialogHeaderComponent,
  ],
  templateUrl: './add-judgment-component.html',
  styleUrl: './add-judgment-component.css',
})
export class AddJudgmentDialog {
  judgmentForm: FormGroup<frmAddJudgment>;
  formSubmitted = false;
  judgmentTypes = Object.values(enJudgmentType);
  judgmentSubTypes = Object.values(enJudgmentSubType);
  caseId: string = '';
  constructor(
    private formBuilder: clsFormsBuilder,
    private toastService: ToasterService,
    private judgmentService: JudgmentService,
    public dialogRef: MatDialogRef<AddJudgmentDialog>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.judgmentForm = this.formBuilder.createAddJudgmentForm();
    this.caseId = this.data.caseId;
  }

  submit() {
    this.judgmentForm.markAllAsTouched();

    if (this.judgmentForm.invalid) return;

    this.judgmentService.add(this.judgmentForm, this.data.caseId).subscribe({
      next: () => {
        this.toastService.success('تم إضافة الحكم بنجاح');
        this.dialogRef.close(true);
      },
      error: (err) => {
        console.error('Judgment add error:', err);
        this.toastService.error('حدث خطأ أثناء إضافة الحكم');
      },
    });
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    this.judgmentForm.controls.file.setValue(file);
    this.judgmentForm.controls.file.markAsTouched();
    this.judgmentForm.controls.file.updateValueAndValidity();
  }
}
