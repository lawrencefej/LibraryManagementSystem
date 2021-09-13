/* tslint:disable */
/* eslint-disable */
import { AddressDto } from './address-dto';
import { MemberGenderDto } from './member-gender-dto';
export interface LibraryCardForUpdate {
  address: AddressDto;
  email: string;
  firstName: string;
  gender: MemberGenderDto;
  id: number;
  lastName: string;
  phoneNumber: string;
}
