import { AbstractControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { StateService } from 'src/app/_services/state.service';
import { StateDto } from 'src/dto/models';

export function stateValidator(control: AbstractControl) {
  const unsubscribe = new Subject<void>();
  let states: StateDto[] = [];
  const stateService = new StateService();

  stateService
    .getStatesObject()
    .pipe(takeUntil(unsubscribe))
    .subscribe(data => {
      states = data;
    });

  unsubscribe.next();
  unsubscribe.complete();

  const state: StateDto = control.value;

  if (states.some(x => x.id === state.id)) {
    return null;
  } else {
    return { stateValidator: true };
  }
}
