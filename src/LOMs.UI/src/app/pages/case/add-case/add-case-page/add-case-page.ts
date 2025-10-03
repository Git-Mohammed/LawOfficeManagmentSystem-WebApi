import { Component, OnInit } from '@angular/core';
import { PageHeaderComponent } from '../../../../../shared/components/page header/page-header-component/page-header-component';
import {
  FormGroup,
  FormsModule,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule, JsonPipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { CaseService } from '../../services/case-service';
import { ActivatedRoute } from '@angular/router';
import { AddClientComponent } from '../dialogs/add-client/add-client-dialog';
import {
  IAddCaseForm,
  ICourt,
  IemployeeName,
  IExistingClientForm,
  INewClientForm,
} from '../../../../../core/models/requests';
import { clsFormsBuilder } from '../../../../../core/services/formBuilder/clsforms-builder';
import { ToasterService } from '../../../../../core/services/toaster-service';
import { MatMenuModule } from '@angular/material/menu';
import { enErrorCodes } from '../../../../../shared/enums/errors';

@Component({
  selector: 'app-add-case-component',
  standalone: true,
  imports: [
    PageHeaderComponent,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    JsonPipe,
    MatMenuModule,
  ],
  templateUrl: './add-case-page.html',
  styleUrl: './add-case-page.css',
})
export class AddCaseComponent implements OnInit {
  showNatBoxErrors: boolean = false;
  natId: string;
  addCaseForm: FormGroup<IAddCaseForm>;
  courts!: ICourt[];
  employeeNames!: IemployeeName[];

  constructor(
    private dialogof: MatDialog,
    private caseService: CaseService,
    private formsBuilder: clsFormsBuilder,
    private toasterService: ToasterService,
    private fb: NonNullableFormBuilder,
    private activatedRoute: ActivatedRoute
  ) {
    this.natId = '';
    this.addCaseForm = this.formsBuilder.createAddCasesForm();
  }
  ngOnInit(): void {
    this.handelCourtTypes();
    this.handelEmployeeNames();
  }

  private handelCourtTypes() {
    this.courts = this.activatedRoute.snapshot.data['court'] as ICourt[];
  }

  private handelEmployeeNames() {
    this.employeeNames = this.activatedRoute.snapshot.data[
      'employeeNames'
    ] as IemployeeName[];
  }

  search(natId: string, invalid: boolean) {
    this.showNatBoxErrors = true;
    if (invalid) {
      return;
    }
    const isExsit: boolean = this.isClientExsit(natId);
    if (isExsit) {
      this.toasterService.error('العميل بالفعل مضاف في القضية');
      return;
    } else {
      this.caseService.getClientByNatId(natId).subscribe({
        next: (res) => {
          const existingClient = this.fb.group<IExistingClientForm>({
            Id: this.fb.control(res.id),
            natId: this.fb.control(res.person.value.natId!),
          });

          this.addCaseForm.controls.existingClients?.push(existingClient);
          this.natId = '';
          this.showNatBoxErrors = false;
        },
        error: (error) => {
          this.dialogof
            .open(AddClientComponent, {
              // height: '325px',
              minWidth: '600px',
              data: { NatId: natId },
            })
            .afterClosed()
            .subscribe({
              next: (result: FormGroup<INewClientForm>) => {
                if (result) {
                  if (result instanceof FormGroup) {
                    if (
                      !this.addCaseForm.value.newClients?.some(
                        (x) => result.value.person?.natId === x.person?.natId
                      )
                    ) {
                      this.addCaseForm?.controls.newClients?.push(result);
                      this.natId = '';
                      this.showNatBoxErrors = false;
                    }
                  }
                }
              },
            });
        },
      });
    }

    return;
  }

  private isClientExsit(natId: string) {
    if (
      this.addCaseForm.value.existingClients?.find((c) => c.natId == natId) !=
      undefined
    )
      return true;
    else
      return (
        this.addCaseForm.value.newClients?.find(
          (c) => c.person?.natId == natId
        ) != undefined
      );
  }

  deleteNewClient(idx: number) {
    this.addCaseForm.controls.newClients?.removeAt(idx);
  }
  deleteExistingClient(idx: number) {
    this.addCaseForm.controls.existingClients?.removeAt(idx);
  }

  submit(isDraft: boolean) {
    this.addCaseForm.markAllAsTouched();
    if (
      !(
        this.addCaseForm.value.existingClients?.length ||
        this.addCaseForm.value.newClients?.length
      )
    )
      this.toasterService.error('يجب ان يكون هناك عميل واحد على الاقل');
    else if (!this.addCaseForm.invalid) {
      this.caseService.add(this.addCaseForm, isDraft).subscribe({
        next: (res) => {
          this.toasterService.success('تمت إضافة القضية بنجاح');
          this.addCaseForm.controls.newClients.clear();
          this.addCaseForm.controls.existingClients.clear();
          this.addCaseForm.reset();
          this.addCaseForm.markAsUntouched();
        },
        error: (err) => {
          console.log('error during adding case', err);
          if (err.code != undefined) {
            const errorCode = err.code as enErrorCodes;
            switch (errorCode) {
              case enErrorCodes.duplicatePhone:
                this.toasterService.error('يوجد رقم مكرر في احد اطراف القضية');
                break;
              default:
                this.toasterService.error('خطا في اضافة القضية');
            }
          }
        },
      });
    }
  }
}
