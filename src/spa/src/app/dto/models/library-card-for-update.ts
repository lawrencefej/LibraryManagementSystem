/* tslint:disable */
/* eslint-disable */
import { AddressDto } from './address-dto';
export interface LibraryCardForUpdate {
  address?: AddressDto;
  email: string;
  firstName: string;
  gender?: null | string;
  id?: number;
  lastName: string;
  phoneNumber: string;
  status?: null | string;
}
