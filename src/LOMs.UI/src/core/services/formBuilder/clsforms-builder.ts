import { FormGroup, Validators, NonNullableFormBuilder } from '@angular/forms';
import { Injectable } from '@angular/core';
import { featureValidator } from '../../../shared/validators/Date/feature-date-validator';
import { minAgeValidator } from '../../../shared/validators/minuimum-date.validator';
import {
  frmAddNewCase,
  INewClientForm,
  IExistingClientForm,
  INewPersonForm,
  IAddCaseForm,
  INewEmployee,
  frmAddContract,
  IAddSessionForm,
  IAddPOAForm,
  IAddAttachmetnForm,
  frmAddJudgment,
  frmAddTemplate,
  frmChangePassword,
} from '../../models/requests';
import { ClsHelpers } from '../../../shared/util/helpers/cls-helpers';
import { enContractType } from '../../../shared/enums/contract-types';
import { contractDateValidator } from '../../../shared/validators/Date/contract-dates-validator';
import { FileValidator } from '../../../shared/validators/files/file-validator';
import { enJudgmentSubType, enJudgmentType } from '../../../shared/enums/enums';
import { passwordPolicyValidator } from '../../../shared/validators/password/password-validator';
import { enRole } from '../../../shared/enums/roles';

@Injectable({
  providedIn: 'root',
})
export class clsFormsBuilder {
  createAddJudgmentForm(): FormGroup<frmAddJudgment> {
    return this.fb.group({
      number: this.fb.control('', Validators.required),
      type: this.fb.control(enJudgmentType.FTDegree, Validators.required),
      subType: this.fb.control(enJudgmentSubType.normal, {
        validators: Validators.required,
      }),
      issueDate: this.fb.control(this.helper.formatDateForInput(new Date()), {
        validators: [Validators.required],
      }),
      file: this.fb.control<File | null>(null, {
        validators: [Validators.required, FileValidator.validate()],
      }),
    });
  }

  constructor(private fb: NonNullableFormBuilder, private helper: ClsHelpers) {}
  createAddAttachmentForm(): FormGroup<IAddAttachmetnForm> {
    return this.fb.group({
      name: this.fb.control('', { validators: [Validators.required] }),
      attachmentFile: this.fb.control<File | null>(null, {
        validators: [Validators.required, FileValidator.validate()],
      }),
    });
  }

  frmAddTemplate(): FormGroup<frmAddTemplate> {
    return this.fb.group({
      name: this.fb.control('', { validators: [Validators.required] }),
      file: this.fb.control<File | null>(null, {
        validators: [Validators.required, FileValidator.validate()],
      }),
    });
  }

  frmChangePassword(): FormGroup<frmChangePassword> {
    return this.fb.group({
      new: this.fb.control('', { validators: [Validators.required, passwordPolicyValidator] }),
      current: this.fb.control('', { validators: [Validators.required] }),
      username: this.fb.control('', {
        validators: [Validators.required, Validators.pattern(/^\d{10}/)],
      }),
    });
  }

  createCaseForm(): FormGroup<frmAddNewCase> {
    return this.fb.group({
      subject: this.fb.control('', Validators.required),
      clientRequests: this.fb.control('', Validators.required),
      partiesToTheCase: this.fb.control('مدعي', {
        validators: [Validators.required],
      }),
      estimatedTime: this.fb.control(
        this.helper.formatDateForInput(new Date()),
        {
          validators: [Validators.required, featureValidator],
        }
      ),
      courtType: this.fb.control('', Validators.required),
      assignedOfficer: this.fb.control('', Validators.required),
      caseNumber: this.fb.control(''),
      lawyerOpinion: this.fb.control('', Validators.required),
    });
  }

  createNewClientForm(): FormGroup<INewClientForm> {
    return this.fb.group({
      person: this.createPersonForm(),
    });
  }

  createNewEmployeeForm(): FormGroup<INewEmployee> {
    return this.fb.group({
      person: this.createPersonForm(undefined, 18),
      email: this.fb.control('', {
        validators: [Validators.email, Validators.required],
      }),
      role: this.fb.control(enRole.ExecutiveManager, { validators: [Validators.required] }),
    });
  }

  createExistingClientForm(): FormGroup<IExistingClientForm> {
    return this.fb.group({
      Id: this.fb.control('', { validators: [Validators.required] }),
      natId: this.fb.control('', Validators.required),
    });
  }

  createPersonForm(
    obj?: INewPersonForm,
    minuimumAge: number = 1
  ): FormGroup<INewPersonForm> {
    return this.fb.group({
      name: this.fb.control(obj?.name || '', {
        validators: [Validators.required, Validators.minLength(3)],
      }),
      natId: this.fb.control(obj?.natId || '', {
        validators: [Validators.required, Validators.pattern(/^\d{10}/)],
      }),
      birthDate: this.fb.control(
        obj?.birthDate ||
          this.helper.formatDateForInput(
            new Date(
              new Date().getFullYear() - minuimumAge,
              new Date().getMonth(),
              new Date().getDate()
            )
          ),
        {
          validators: [Validators.required, minAgeValidator(minuimumAge)],
        }
      ),
      phone: this.fb.control<string>(obj?.phone || '', {
        validators: [Validators.required, Validators.pattern(/^5\d{8}$/)],
      }),
      address: this.fb.control(obj?.address || '', {
        validators: [Validators.required, Validators.minLength(10)],
      }),
      countryCode: this.fb.control(obj?.countryCode || 'SA', {
        validators: [Validators.required],
      }),
    });
  }

  createAddCasesForm(): FormGroup<IAddCaseForm> {
    return this.fb.group({
      case: this.createCaseForm(),
      existingClients: this.fb.array<FormGroup<IExistingClientForm>>([]),
      newClients: this.fb.array<FormGroup<INewClientForm>>([]),
    });
  }

  createAddContractForm(): FormGroup<frmAddContract> {
    return this.fb.group(
      {
        assigned: this.fb.control(false),
        contractType: this.fb.control(enContractType.FixedTerm),
        totalPrice: this.fb.control(0, Validators.min(0)),
        downAmount: this.fb.control(0, Validators.min(0)),
        expirationDate: this.fb.control(
          this.helper.formatDateForInput(new Date()),
          Validators.required
        ),
        issueDate: this.fb.control(
          this.helper.formatDateForInput(new Date()),
          Validators.required
        ),
        contractAttachment: this.fb.control<File | null>(null, {
          validators: [Validators.required, FileValidator.validate()],
        }),
      },
      { validators: [contractDateValidator()] }
    );
  }

  createAddPOAForm(): FormGroup<IAddPOAForm> {
    return this.fb.group({
      poaNumber: this.fb.control('', { validators: [Validators.required] }),
      poaAuthrizedBy: this.fb.control('', {
        validators: [Validators.required],
      }),
      poaIssueDate: this.fb.control(
        this.helper.formatDateForInput(new Date()),
        {
          validators: [Validators.required],
        }
      ),
      poaAttachment: this.fb.control<File | null>(null, {
        validators: [Validators.required, FileValidator.validate()],
      }),
    });
  }

  createAddSessionForm(): FormGroup<IAddSessionForm> {
    return this.fb.group({
      date: this.fb.control(this.helper.formatDateForInput(new Date()), {
        validators: [Validators.required, featureValidator],
      }),
      layerId: this.fb.control('', Validators.required),
      tasks: this.fb.control('', Validators.required),
    });
  }
}
