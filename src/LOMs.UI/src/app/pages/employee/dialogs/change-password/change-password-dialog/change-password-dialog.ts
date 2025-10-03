import { Component, Inject } from '@angular/core';
import { frmChangePassword } from '../../../../../../core/models/requests';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { AuthManagement } from '../../../../../../core/services/auth/auth-management';
import { ToasterService } from '../../../../../../core/services/toaster-service';
import { DialogHeaderComponent } from '../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';

@Component({
  selector: 'app-change-password-dialog',
  imports: [ReactiveFormsModule, MatDialogModule, DialogHeaderComponent],
  templateUrl: './change-password-dialog.html',
  styleUrl: './change-password-dialog.css'
})
export class ChangePasswordDialog {
  frmChangePassword!: FormGroup<frmChangePassword>;

  constructor(
    private myfb: clsFormsBuilder,
    public dialogRef: MatDialogRef<ChangePasswordDialog>,
    private authService: AuthManagement,
    private toastService: ToasterService,
    @Inject(MAT_DIALOG_DATA) private data: any,
  ) {
    this.frmChangePassword = this.myfb.frmChangePassword();
  }

  submit() {
    this.frmChangePassword.markAllAsTouched();

    if (!this.frmChangePassword.invalid) {

      this.authService.changePassword(this.frmChangePassword).subscribe({
        next: () => {
          this.toastService.success('تم تغيير كلمة المرور بنجاح');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Change password error:', error);
          this.toastService.error('حدث خطأ أثناء تغيير كلمة المرور');
        },
      });
    }
  }
}
