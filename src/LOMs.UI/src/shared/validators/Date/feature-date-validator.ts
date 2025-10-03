import { AbstractControl, ValidationErrors } from '@angular/forms';

export function featureValidator(
  control: AbstractControl
): ValidationErrors | null {
  const value = new Date(control.value);
  if (!value) return null;

  const today = new Date();
  return value.getTime() > today.getTime() ? null : { pastDate: true };
}
