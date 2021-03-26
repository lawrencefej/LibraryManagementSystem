import { AbstractControl } from '@angular/forms';

export function ConfirmPasswordValidator(control: AbstractControl) {
  const password: string = control.get('password').value;
  const confirmPassword: string = control.get('confirmPassword').value;

  if (confirmPassword === '') {
    return;
  }

  if (password !== confirmPassword) {
    control.get('confirmPassword').setErrors({ confirmPasswordMatch: true });
  }
}
