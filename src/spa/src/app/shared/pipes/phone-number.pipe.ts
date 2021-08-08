import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneNumber'
})
export class PhoneNumberPipe implements PipeTransform {
  // TODO test and delete
  // transform(phoneNumber: number): string {
  //   const str = phoneNumber.toString();
  //   return `(${str.slice(0, 3)}) ${str.slice(3, 6)}-${str.slice(6)}`;
  // }
  transform(phoneNumber: string): string {
    // const phoneNumber = phoneNumber.toString();
    return `(${phoneNumber.slice(0, 3)}) ${phoneNumber.slice(3, 6)}-${phoneNumber.slice(6)}`;
  }
}
