/* tslint:disable */
/* eslint-disable */
import { AddressDto } from './address-dto';
import { CheckoutForListDto } from './checkout-for-list-dto';
export interface LibraryCardForDetailedDto {
  address: AddressDto;
  addressId: number;
  age: number;
  cardNumber: string;
  checkouts: Array<CheckoutForListDto>;
  created: string;
  email: string;
  fees: number;
  firstName: string;
  gender: string;
  id: number;
  lastName: string;
  libraryCardPhoto: string;
  phoneNumber: string;
  photoUrl: string;
  status: string;
}
