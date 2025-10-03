// validators/conditional-date.validator.ts
import {
  AbstractControl,
  ValidationErrors,
  ValidatorFn,
  FormGroup,
} from '@angular/forms';
import { enContractType } from '../../enums/contract-types';
import { frmAddContract } from '../../../core/models/requests';

export function contractDateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as FormGroup<frmAddContract>;

    const errors: ValidationErrors = {};
    if (formGroup.value.contractType === enContractType.FixedTerm) {
      // Replace with your actual enum value

      // Validate issue date
      if (!formGroup.value.issueDate) {
        errors['issueDateRequired'] = true;
      }

      // Validate expiration date
      if (!formGroup.value.expirationDate) {
        errors['expirationDateRequired'] = true;
      }

      // Validate date logic (expiration > issue)
      if (formGroup.value.issueDate && formGroup.value.expirationDate) {
        const issue = new Date(formGroup.value.issueDate);
        const expiration = new Date(formGroup.value.expirationDate);

        if (expiration <= issue) {
          errors['invalidDateRange'] = true;
        }
      }
      
    }
    
    // Validate expiration date
    if (
      Number(formGroup?.value?.downAmount) >
      Number(formGroup.value.totalPrice)
    ) {
      console.log('the down amount is bigger');
      errors['downAmountExceeded'] = true;
    }
    return Object.keys(errors).length > 0 ? errors : null;
  };
}
