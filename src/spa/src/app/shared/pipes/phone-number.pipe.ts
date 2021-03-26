import { Pipe, PipeTransform, } from '@angular/core';

@Pipe({
  name: 'phoneNumber'
})
export class PhoneNumberPipe implements PipeTransform {

  transform(value: any): string {
    const str = value.toString();
    return `(${str.slice(0, 3)}) ${str.slice(3, 6)}-${str.slice(6)}`;
  }

}
