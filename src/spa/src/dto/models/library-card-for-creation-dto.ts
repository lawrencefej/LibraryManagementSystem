/* tslint:disable */
/* eslint-disable */
import { AddAddressDto } from './add-address-dto';
import { MemberGenderDto } from './member-gender-dto';
export interface LibraryCardForCreationDto {
  address: AddAddressDto;
  dateOfBirth: string;
  email: string;
  firstName: string;
  gender: MemberGenderDto;
  lastName: string;
  phoneNumber: string;
}
