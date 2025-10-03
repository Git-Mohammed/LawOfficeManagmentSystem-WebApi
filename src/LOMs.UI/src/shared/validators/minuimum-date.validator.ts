import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function minAgeValidator(minAge: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) return null;

    const birthDate = new Date(control.value);
    if (isNaN(birthDate.getTime())) return { invalidDate: true };

    const today = new Date();
    const age = today.getFullYear() - birthDate.getFullYear();

    const hasHadBirthdayThisYear =
      today.getMonth() > birthDate.getMonth() ||
      (today.getMonth() === birthDate.getMonth() &&
        today.getDate() >= birthDate.getDate());

    const actualAge = hasHadBirthdayThisYear ? age : age - 1;

    return actualAge >= minAge
      ? null
      : {
          minAge: {
            requiredAge: minAge,
            actualAge: actualAge,
          },
        };
  };
}
