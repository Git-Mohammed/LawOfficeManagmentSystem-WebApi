import { Component, Inject, OnInit } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import {
  FormControl,
  FormGroup,
  FormsModule,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { JsonPipe, NgIf, NgFor } from '@angular/common';

import { Country, CountrySelectComponent } from '@wlucha/ng-country-select';
import { ToasterService } from '../../../../../../core/services/toaster-service';
import { ICounty, INewClientForm } from '../../../../../../core/models/requests';
import { clsFormsBuilder } from '../../../../../../core/services/formBuilder/clsforms-builder';
import { DialogHeaderComponent } from '../../../../../../shared/components/dialog-header/dialog-header-component/dialog-header-component';
import { ClsHelpers } from '../../../../../../shared/util/helpers/cls-helpers';
import { EmployeeService } from '../../../../employee/services/employee-service';
import { ClientService } from '../../../../../../core/services/client/client-service';

@Component({
  selector: 'app-add-client-component',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    NgIf,
    JsonPipe,
    NgFor,
    CountrySelectComponent,
    DialogHeaderComponent,
  ],
  templateUrl: './add-client-dialog.html',
  styleUrl: './add-client-dialog.css',
})
export class AddClientComponent implements OnInit {
  clientForm: FormGroup<INewClientForm>;
  counties!: ICounty[];
  constructor(
    private helper: ClsHelpers,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AddClientComponent>,
    private formBuilder: clsFormsBuilder,
    private toastService: ToasterService,
    private fb: NonNullableFormBuilder,
    private clientService: ClientService
  ) {
    this.clientForm = this.formBuilder.createNewClientForm();
  }

  ngOnInit(): void {
    if (this.clientForm.value.person && this.data.NatId) {
      this.clientForm.controls.person.get('natId')?.setValue(this.data.NatId);
    }

    this.counties = this.helper.loadCountries();
  }

  submit() {
    this.clientForm.markAllAsTouched();
    if (!this.clientForm.invalid) {
      this.dialogRef.close(this.clientForm);
    }
  }
}
