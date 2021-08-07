import { AbstractControl, ValidatorFn } from '@angular/forms';

export function ConfirmPasswordValidator(control: AbstractControl): void {
  const password: string = control.get('password')?.value;
  const confirmPassword: string = control.get('confirmPassword')?.value;

  if (confirmPassword === '') {
    return;
  }

  if (password !== confirmPassword) {
    control.get('confirmPassword')?.setErrors({ confirmPasswordMatch: true });
  }
}

// custom validator to check that two fields match
// export function MustMatch(controlName: string, matchingControlName: string): (formGroup: FormGroup) => void {
//   return (formGroup: FormGroup) => {
//     const control = formGroup.controls[controlName];
//     const matchingControl = formGroup.controls[matchingControlName];

//     if (matchingControl.errors && !matchingControl.errors.mustMatch) {
//       // return if another validator has already found an error on the matchingControl
//       return;
//     }

//     // set error on matchingControl if validation fails
//     if (control.value !== matchingControl.value) {
//       matchingControl.setErrors({ mustMatch: true });
//     } else {
//       matchingControl.setErrors(null);
//     }
//   };
// }

// custom validator to check that two fields match
export function MustMatch(controlName: string, matchingControlName: string): ValidatorFn {
  return (controls: AbstractControl) => {
    const control = controls.get(controlName);
    const checkControl = controls.get(matchingControlName);

    if (checkControl.errors && !checkControl.errors.matching) {
      return null;
    }

    if (control.value !== checkControl.value) {
      controls.get(matchingControlName).setErrors({ matching: true });
      return { matching: true };
    } else {
      return null;
    }
  };
}
