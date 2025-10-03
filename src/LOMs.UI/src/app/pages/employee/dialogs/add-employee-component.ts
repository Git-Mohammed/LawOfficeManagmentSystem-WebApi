import { JsonPipe, NgIf } from '@angular/common';
import { Component, inject, Inject, OnInit, signal, Signal } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialogClose,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { EmployeeService } from '../services/employee-service';
import { clsFormsBuilder } from '../../../../core/services/formBuilder/clsforms-builder';
import {
  ICounty,
  IEmployeeDetails,
  INewEmployee,
} from '../../../../core/models/requests';
import { MatDialogContent } from '../../../../../node_modules/@angular/material/dialog/index';
import { DialogHeaderComponent } from '../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';
import { ClsHelpers } from '../../../../shared/util/helpers/cls-helpers';
import { ToasterService } from '../../../../core/services/toaster-service';
import { enDialogMode } from '../../../../shared/enums/dialog-mode';
import { enErrorCodes } from '../../../../shared/enums/errors';
import { SessionManagement } from '../../../../core/services/session/session-management';
import { enRole } from '../../../../shared/enums/roles';

@Component({
  selector: 'app-add-employee-component',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    JsonPipe,
    MatDialogClose,
    NgIf,
    MatDialogModule,
    DialogHeaderComponent,
  ],
  templateUrl: './add-employee-component.html',
  styleUrl: './add-employee-component.css',
})
export class AddEmployeeComponent {
  employeeForm!: FormGroup<INewEmployee>;
  mode!: enDialogMode;
  countries!: ICounty[];
  title!: string;
  employeeId!: string;
  usedNatId!: string;
  usedPhone!: string;
  usedEmail!: string;
  session = inject(SessionManagement)
  mangerUpdate = signal((this.session.getSession() ? this.session.getSession()?.role == enRole.GeneralManager : false) && this.mode == enDialogMode.update)

  constructor(
    private employeeService: EmployeeService,
    private formsBuilder: clsFormsBuilder,
    public dialogRef: MatDialogRef<AddEmployeeComponent>,
    private helper: ClsHelpers,
    private toasterService: ToasterService,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.mode = this.data.mode as enDialogMode;
    this.employeeId = this.data.employeeId;
    this.initialize();
  }

  IsPhoneDuplicated = signal(false);
  IsNatDuplicated = signal(false);
  IsEmailDuplicated = signal(false);

  submit() {
    this.employeeForm.markAllAsTouched();
    if (!this.employeeForm.invalid) {
      switch (this.mode) {
        case enDialogMode.add:
          {
            this.employeeService.add(this.employeeForm).subscribe({
              next: (res) => {
                this.toasterService.success('تمت إضافة الموظف بنجاح');
                this.dialogRef.close();
              },
              error: (err) => {
                if (err.code != undefined) {
                  const errorCode = err.code as enErrorCodes;
                  switch (errorCode) {
                    case enErrorCodes.duplicateNatId:
                      this.IsNatDuplicated.set(true);
                      this.usedNatId =
                        this.employeeForm.value.person?.natId ?? '';
                      break;

                    case enErrorCodes.duplicatePersonPhone:
                      this.IsPhoneDuplicated.set(true);
                      this.usedPhone =
                        this.employeeForm.value.person?.phone ?? '';
                      break;

                    case enErrorCodes.duplicateEmail:
                      this.IsEmailDuplicated.set(true);
                      this.usedEmail = this.employeeForm.value.email ?? '';
                      break;
                  }

                  this.employeeForm.markAllAsTouched();
                } else {
                  this.toasterService.error('حدث خطأ اثناء اضافة الموظف');
                }
                console.log('error during adding client', err);
              },
            });
          }
          break;

        case enDialogMode.update:
          {
            debugger
            this.employeeService
              .update(this.employeeForm, this.employeeId, true)
              .subscribe({
                next: (res) => {
                  this.toasterService.success('تمت تحديث بيانات الموظف بنجاح');
                  this.dialogRef.close();
                },
                error: (err) => {
                  this.toasterService.error(
                    'حدث خطأ اثناء تحديث بيانات الموظف'
                  );
                  console.log('error during update employee', err);
                },
              });
          }
          break;
      }
    }
  }

  private initialize() {
    this.employeeForm = this.formsBuilder.createNewEmployeeForm();
    this.countries = this.helper.loadCountries();
    switch (this.mode) {
      case enDialogMode.add:
        this.title = 'إضافة موظف جديد';

        break;
      case enDialogMode.update:
        {
          this.title = 'تعديل بيانات الموظف';
          this.employeeService.getById(this.employeeId).subscribe({
            next: (data) => {
              console.log('employee details', data);
              this.employeeForm.controls.email.setValue(data.email);
              this.employeeForm.controls.role.setValue(data.role);
              this.employeeForm.controls.person.controls.name.setValue(
                data.fullName
              );
              this.employeeForm.controls.person.controls.address.setValue(
                data.address
              );
              this.employeeForm.controls.person.controls.birthDate.setValue(
                this.helper.formatDateForInput(new Date(data.birthDate))
              );
              this.employeeForm.controls.person.controls.natId.setValue(
                data.nationalId
              );
              this.employeeForm.controls.person.controls.countryCode.setValue(
                data.countryCode
              );
              this.employeeForm.controls.person.controls.phone.setValue(
                data.phone
              );
            },
            error: (error) => {
              console.log('Error details:', error);
              this.toasterService.error('حدث خطا في تحديث بيانات الموظف');
              this.dialogRef.close();
            },
          });
        }
        break;
    }
  }

}
