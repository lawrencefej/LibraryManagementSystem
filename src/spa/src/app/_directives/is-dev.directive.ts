import { Directive } from '@angular/core';

@Directive({
  selector: '[appIsDev]'
})
export class IsDevDirective {
  constructor() {}
}
