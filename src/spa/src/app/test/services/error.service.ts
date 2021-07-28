import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

export interface TestError {
  form: FormGroup;
  errors: string[];
}

@Injectable()
export class ErrorService {
  constructor() {}

  mapErrors(form: FormGroup, error: any): TestError {
    const returnErrors: string[] = [];
    const validationErrors = error.error.errors;

    Object.keys(validationErrors).forEach(prop => {
      const propErrors: string[] = validationErrors[prop];

      const formControl = form.get(prop.toLowerCase());

      if (formControl) {
        propErrors.push('test error');
        formControl.setErrors({ serverError: propErrors });
      } else {
        returnErrors.push(`${prop}: ${propErrors}`);
      }
    });
    return { form, errors: returnErrors };
  }
}
