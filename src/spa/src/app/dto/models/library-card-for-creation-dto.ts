/* tslint:disable */
/* eslint-disable */
import { AddressDto } from './address-dto';
import { MemberGenderDto } from './member-gender-dto';
export interface LibraryCardForCreationDto {
  address: AddressDto;
  dateOfBirth?: string;
  email?: null | string;
  firstName?: null | string;
  gender: MemberGenderDto;
  lastName?: null | string;
  phoneNumber?: null | string;
}
