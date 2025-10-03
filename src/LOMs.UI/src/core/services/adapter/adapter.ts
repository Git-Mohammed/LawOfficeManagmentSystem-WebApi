import { FormGroup, NonNullableFormBuilder } from '@angular/forms';
import {
  INewClientForm,
  IClientDetails,
  IemployeeName,
  IAddCaseForm,
  frmAddContract,
  ICourtDetaills,
  ICaseRow,
  IListDTO,
  IContractRow,
  ISessionsRow,
  IAddPOAForm,
  IPOARow,
  IAddAttachmetnForm,
  IAddSessionForm,
  sessionDetails,
  INewPersonForm,
  INewEmployee,
  IEmployeeDetails,
  frmAddJudgment,
  IJudgmentRow,
  frmAddTemplate,
  frmChangePassword,
  IEmployeeRow,
  ITemplateBox,
  IAttachmentRow,
} from '../../models/requests';
import {
  ICseGeneralDetails,
  ICasesParties,
} from '../../../app/pages/case/case details/components/case-details-component/case-details-component';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class Adapter {
  employeeRowAdapter(row: any): IEmployeeRow {
    return {
      id: row.id,
      name: row.name,
      natId: row.natId,
      countryCode: row.countryCode,
      role: row.role,
      email: row.email,
      birthDate: row.birthDate,
      phone: row.phone,
      IsActive: row.isActive,
    };
  }
  frmChangePasswordAdapter(
    frmChangePassword: FormGroup<frmChangePassword>
  ): any {
    return {
      username: frmChangePassword?.value?.username,
      currentPassword: frmChangePassword?.value?.current,
      newPassword: frmChangePassword?.value?.new,
    };
  }
  judgmentRowAdapter(res: any): IJudgmentRow {
    return {
      id: res.judgmentId,
      caseId: res.caseId,
      type: res.type,
      subType: res.subType,
      number: res.number,
      issueDate: res.issuedOn,
      creatorName: res.employeeNameCreatedBy,
      createdDate: res.createdDate,
      file: {
        filePath: res?.file.filePath ?? 'is not working',
        fileName: res?.file.fileName,
        fileType: res?.file.fileType,
      },
    };
  }
  constructor(private fb: NonNullableFormBuilder) {}

  getCaseAttachmentRowAdapter(row: any): IAttachmentRow {
    return {
      id: row?.attachmentId ?? '',
      name: row?.attachmentName ?? '',
      createdDate: row?.createdDate ?? '',
      CreatedBy: row?.createdByEmployeeName ?? '',
      file: {
        filePath: row?.file.filePath ?? 'is not working',
        fileName: row?.file.fileName,
        fileType: row?.file.fileType,
      },
    };
  }

  toAddAttachmentFormAPI(frmAddAttachment: FormGroup<IAddAttachmetnForm>) {
    const formData = new FormData();
    const { name, attachmentFile } = frmAddAttachment.controls;

    formData.append('AttachmentName', name.value);
    if (attachmentFile.value) {
      formData.append('File', attachmentFile.value);
    }

    return formData;
  }
  /**
   *
   */
  toAddClientAPI(model: FormGroup<INewClientForm>): any {
    return {
      kind: 'new',
      Client: {
        person: {
          fullName: model.value.person?.name,
          nationalId: model.value.person?.natId,
          birthDate: model.value.person?.birthDate,
          phone: {
            number: model.value.person?.phone,
            code: '+966',
          },
          address: model.value.person?.address,
          countryCode: model.value.person?.countryCode,
        },
      },
    };
  }

  fromClientDetailsAPI(model: any): IClientDetails {
    return {
      id: model.clientId,
      person: this.fb.group({
        id: this.fb.control(model.person.personId),
        name: this.fb.control(model.person.fullName),
        natId: this.fb.control(model.person.nationalId),
        birthDate: this.fb.control(model.person.birthDate),
        phone: this.fb.control(model.person.phoneNumber),
        address: this.fb.control(model.person.address),
        countryCode: this.fb.control(model.person.countryCode),
      }),
    };
  }

  fromCourtDetailsAPI(data: any): ICourtDetaills {
    return {
      courtTypeId: data?.courtTypeId,
      name: data?.name,
      code: data?.code,
      description: data?.description,
      years: ['1447'],
    };
  }

  fromEmployeeNamesAPI(model: any): IemployeeName {
    return {
      id: model.employeeId,
      name: model.fullName,
    };
  }

  getCaseContractRowAdapter(res: any): IContractRow {
    return {
      id: res?.contractId,
      caseId: res.caseId,
      contractNumber: res?.contractNumber,
      createdDate: res?.createdDate,
      employeeNameCreatedBy: res?.employeeNameCreatedBy ?? 'مافي اسم',
      expirationDate: res?.expiryDate ?? '0000-00-00',
      issueDate: res?.issueDate ?? '0000-00-00',
      totalAmount: res?.totalAmount,
      contractType: res?.contractType,
      file: {
        filePath: res?.file.filePath ?? 'is not working',
        fileName: res?.file.fileName,
        fileType: res?.file.fileType,
      },
    };
  }
  getCasePOARowAdapter(res: any): IPOARow {
    return {
      id: res.poaId,
      caseId: res.caseId,
      number: res.poaNumber,
      createdDate: res.createdDate,
      creatorName: res.employeeNameCreatedBy,
      publisherName: res.issuingAuthority,
      issueDate: res.issueDate,
      file: {
        filePath: res?.file.filePath ?? 'is not working',
        fileName: res?.file.fileName,
        fileType: res?.file.fileType,
      },
    };
  }
  fromCaseDetailsAPI(data: any): ICseGeneralDetails {
    const parties: ICasesParties[] = Array.isArray(data.clients)
      ? data.clients.map(
          (client: any) =>
            ({
              clientId: client.clientId,
              name: client.fullName,
              natId: client.nationalId,
              phoneNumber: client.phoneNumber.number,
              countryCode: client.countryCode,
              address: client.address,
              birthDate: client.birthdate,
            } as ICasesParties)
        )
      : [];

    return {
      caseId: data.caseId,
      caseNumber: data.caseNumber,
      courtTypeName: data.courtTypeName,
      assignedEmployeeName: data.assignedEmployeeName,
      status: data.status,
      caseSubject: data.caseSubject,
      clientRequests: data.clientRequests,
      employeeName: data.employeeName,
      createdDate: data.createdAt,
      estimatedDate: data.estimatedReviewDate,
      layerOpinion: data.lawyerOpinion,
      caseParities: parties,
    };
  }

  toAddCaseAPI(creaCaseForm: FormGroup<IAddCaseForm>, isDraft: boolean): any {
    const formValue = creaCaseForm.value; // safer than .value for disabled fields
    const newClients = formValue.newClients?.map((clientGroup) => ({
      kind: 'new',
      client: {
        person: {
          fullName: clientGroup?.person?.name,
          nationalId: clientGroup?.person?.natId,
          birthDate: clientGroup?.person?.birthDate,
          phone: {
            number: clientGroup?.person?.phone,
            code: '+966',
          },
          address: clientGroup?.person?.address,
          countryCode: clientGroup?.person?.countryCode,
        },
      },
    }));

    const existingClients = formValue.existingClients?.map(
      (existingClient) => ({
        kind: 'existing',
        clientId: existingClient.Id,
      })
    );

    const allClients = [...(newClients ?? []), ...(existingClients ?? [])];

    const body = {
      caseNumber: formValue.case?.caseNumber,
      courtTypeId: formValue.case?.courtType,
      caseSubject: formValue.case?.subject,
      partyRole: String(formValue.case?.partiesToTheCase),
      estimatedReviewDate: formValue.case?.estimatedTime,
      isDraft: isDraft,
      lawyerOpinion: formValue.case?.lawyerOpinion,
      assignedEmployeeId: formValue.case?.assignedOfficer,
      clientRequestDetails: formValue.case?.clientRequests,
      clients: allClients,
    };

    return body;
  }
  toAddContractFormAPI(contractFrm: FormGroup<frmAddContract>): FormData {
    const form = new FormData();
    // Add basic fields with proper conversion
    form.append('ContractType', String(contractFrm.value.contractType));
    form.append('InitialPayment', String(contractFrm.value?.downAmount));
    form.append('BaseAmount', String(contractFrm.value.totalPrice));
    form.append('IsAssigned', String(contractFrm.value.assigned));

    // Handle dates properly

    if (contractFrm.value.expirationDate) {
      form.append('ExpiresOn', String(contractFrm.value.expirationDate));
    }
    if (contractFrm.value.issueDate) {
      form.append('IssuedOn', String(contractFrm.value.issueDate));
    }

    if (contractFrm.value.contractAttachment) {
      const file = contractFrm.value.contractAttachment;
      form.append('ContractFile', file);
    }
    return form;
  }

  toAddPOAFormAPI(frmAddPoa: FormGroup<IAddPOAForm>): FormData {
    const form = new FormData();

    form.append('POANumber', String(frmAddPoa.value.poaNumber));
    form.append('IssueDate', String(frmAddPoa.value.poaIssueDate));
    form.append('IssuingAuthority', String(frmAddPoa.value.poaAuthrizedBy));
    if (frmAddPoa.value.poaAttachment)
      form.append('AttachmentFile', frmAddPoa.value.poaAttachment);

    return form;
  }

  getListDTOAdapter<Type>(
    data: any,
    itemAdapter: (row: any) => Type
  ): IListDTO<Type> {
    return {
      pageIndex: data.pageIndex,
      pageSize: data.pageSize,
      totalItemsCount: data.totalItemsCount,
      itemsCount: data.itemsCount,
      hasPreviousPage: data.hasPreviousPage,
      hasNextPage: data.hasNextPage,
      totalPages: data.totalPages,
      items: data.items.map((row: any) => itemAdapter(row)),
    };
  }
  caseListRowAdapter(row: any): ICaseRow {
    return {
      caseId: row.caseId,
      caseNumber: row.caseNumber || 'N/A',
      fileNumber: row.fileNumber,
      status: row.status,
      caseSubject: row.caseSubject,
      createdDate: row.createdDate,
      courtTypeName: row.courtTypeName,
      employeeName: row.employeeNameCreatedBy ?? row.createdByName,
    };
  }

  getSessionsByCaseIdAdapter(row: any): ISessionsRow {
    return {
      courtSessionId: row.courtSessionId,
      createdDate: new Date(row.createdDate),
      assignedEmployeeName: String(row.assignedEmployeeName),
      createdByEmployeeName: String(row.createdByEmployeeName),
      scheduledAt: new Date(row.scheduledAt),
    };
  }

  createSessionAdapter(data: FormGroup<IAddSessionForm>): any {
    return {
      assignedEmployeeId: data.value.layerId,
      scheduledAt: data.value.date ? new Date(data.value.date) : undefined,
      assignedTasks: data.value.tasks,
    };
  }

  updateSessionAdapter(data: FormGroup<IAddSessionForm>): any {
    return {
      assignedEmployeeId: data.value.layerId,
      newDate: data.value.date,
      assignedTasks: data.value.tasks,
    };
  }
  sessionDetailsAdapter(data: any): sessionDetails {
    return {
      id: data.id,
      caseId: data.caseId,
      assignedEmployeeId: data.assignedEmployeeId,
      assignedEmployeeName: data.assignedEmployeeName,
      scheduledAt: data.scheduledAt,
      assignedTasks: data.assignedTasks,
      isSessionDateExpired: data.isSessionDateExpired,
    };
  }

  updateEmployeeAdapter(data: FormGroup<INewEmployee>): any {
    console.log('employee before update', data);
    return {
      fullName: data.value?.person?.name,
      nationalId: data.value?.person?.natId,
      birthDate: data.value?.person?.birthDate,
      phone: {
        number: data.value?.person?.phone,
        code: '+966',
      },
      address: data.value?.person?.address,
      countryCode: data.value?.person?.countryCode,
      role: data.value?.role,
      email: data.value?.email,
    };
  }

  createEmployeeAdapter(data: FormGroup<INewEmployee>): any {
    return {
      person: {
        fullName: data.value?.person?.name,
        nationalId: data.value?.person?.natId,
        birthDate: data.value?.person?.birthDate,
        phone: {
          number: data.value?.person?.phone,
          code: '+966',
        },
        address: data.value?.person?.address,
        countryCode: data.value?.person?.countryCode,
      },
      role: data.value?.role,
      email: data.value?.email,
    };
  }
  employeeDetailsAdapter(data: any): IEmployeeDetails {
    return {
      id: data.id,
      nationalId: data.natId,
      fullName: data.name,
      email: data.email,
      address: data.address,
      phone: data.phoneNumber,
      role: data.role,
      birthDate: data.birthDate,
      countryCode: data.countryCode,
    };
  }

  frmAddJudgmentAdapter(judgment: FormGroup<frmAddJudgment>): any {
    const form = new FormData();

    form.append('Number', String(judgment.value.number));
    form.append('Type', String(judgment.value.type));
    form.append('SubType', String(judgment.value.subType));
    form.append('IssuedOn', String(judgment.value.issueDate));

    if (judgment.value.file) form.append('File', judgment.value.file);

    return form;
  }

  frmAddTemplateAdapter(frmTemplate: FormGroup<frmAddTemplate>) {
    const form = new FormData();

    form.append('TemplateName', String(frmTemplate.value.name));

    if (frmTemplate.value.file) form.append('File', frmTemplate.value.file);

    return form;
  }

  templateRowAdapter(data: any): ITemplateBox {
    return {
      id: data?.templateId,
      name: data?.templateName,
      file: {
        filePath: data?.file.filePath ?? 'is not working',
        fileName: data?.file.fileName,
        fileType: data?.file.fileType,
      },
    };
  }

  templatesList(data: any): ITemplateBox[] {
    return data.map((row: any) => this.templateRowAdapter(row));
  }

  getAllEmployees(data: any): IEmployeeRow[] {
    return data.map((row: any) => this.employeeRowAdapter(row));
  }
}
