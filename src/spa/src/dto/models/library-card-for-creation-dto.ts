/* tslint:disable */
/* eslint-disable */
import { AddressDto } from './address-dto';
import { MemberGenderDto } from './member-gender-dto';
export interface LibraryCardForCreationDto {
  address: AddressDto;
  dateOfBirth: string;
  email?: null | string;
  firstName: string;
  gender: MemberGenderDto;
  lastName: string;
  phoneNumber: string;
}
