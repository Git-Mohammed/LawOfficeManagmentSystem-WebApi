import { AbstractControl, ValidationErrors } from '@angular/forms';

export function passwordPolicyValidator(
  control: AbstractControl
): ValidationErrors | null {
  const value = control.value as string;

  if (!value) return { required: true };

  const errors: ValidationErrors = {};

  // شروط السياسة
  const requiredLength = 8;
  const requireDigit = true;
  const requireUppercase = true;
  const requireLowercase = true;
  const requireSymbol = true;
  const requiredUniqueChars = 5;

  // تحقق من الطول
  if (value.length < requiredLength) {
    errors['minLength'] = {
      requiredLength,
      actualLength: value.length,
      message: `يجب أن تكون كلمة المرور على الأقل ${requiredLength} أحرف`,
    };
  }

  // تحقق من الرقم
  if (requireDigit && !/\d/.test(value)) {
    errors['digit'] = {
      message: 'يجب أن تحتوي كلمة المرور على رقم واحد على الأقل',
    };
  }

  // تحقق من الحرف الكبير
  if (requireUppercase && !/[A-Z]/.test(value)) {
    errors['uppercase'] = {
      message: 'يجب أن تحتوي كلمة المرور على حرف كبير واحد على الأقل',
    };
  }

  // تحقق من الحرف الصغير
  if (requireLowercase && !/[a-z]/.test(value)) {
    errors['lowercase'] = {
      message: 'يجب أن تحتوي كلمة المرور على حرف صغير واحد على الأقل',
    };
  }

  // تحقق من الرموز
  if (requireSymbol && !/[^\w\s]/.test(value)) {
    errors['symbol'] = {
      message:
        'يجب أن تحتوي كلمة المرور على رمز واحد على الأقل مثل ! أو @ أو #',
    };
  }

  // تحقق من عدد الأحرف الفريدة
  const uniqueChars = new Set(value).size;
  if (uniqueChars < requiredUniqueChars) {
    errors['uniqueChars'] = {
      requiredUniqueChars,
      actualUniqueChars: uniqueChars,
      message: `يجب أن تحتوي كلمة المرور على ${requiredUniqueChars} أحرف فريدة على الأقل`,
    };
  }

  return Object.keys(errors).length ? errors : null;
}
