import { AbstractControl } from '@angular/forms';
import { StateService } from 'src/app/_services/state.service';

export function stateValidator(control: AbstractControl) {
    const states: string[] = new StateService().getStates();
    const state: string = control.value;
    if (states.filter(x => x === state).length === 1 || state === '') {
    return null;
  } else {
    return {stateValidator: true};
  }
}
