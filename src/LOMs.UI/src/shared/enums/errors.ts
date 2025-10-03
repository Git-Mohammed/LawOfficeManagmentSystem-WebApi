export enum enErrorCodes {
  duplicatePhone = 'Case.DuplicatePhoneNumbers',
  duplicatePersonPhone = 'Person.PhoneNumberAlreadyExists',
  duplicateNatId = 'Person.ExistingNationalId',
  duplicateEmail = 'Employee.ExistingEmail'
}

export interface IBusinessError {
  type: string;
  title: string;
  status: number;
  code: enErrorCodes;
}