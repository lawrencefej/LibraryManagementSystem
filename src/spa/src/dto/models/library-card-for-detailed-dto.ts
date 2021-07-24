/* tslint:disable */
/* eslint-disable */
import { AddressDto } from './address-dto';
import { CheckoutForListDto } from './checkout-for-list-dto';
export interface LibraryCardForDetailedDto {
  address?: AddressDto;
  addressId?: number;
  age?: number;
  cardNumber?: null | string;
  checkouts?: null | Array<CheckoutForListDto>;
  created?: string;
  email?: null | string;
  fees?: number;
  firstName?: null | string;
  gender?: null | string;
  id?: number;
  lastName?: null | string;
  libraryCardPhoto?: null | string;
  phoneNumber?: null | string;
  photoUrl?: null | string;
  status?: null | string;
}
