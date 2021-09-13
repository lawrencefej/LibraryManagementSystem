/* tslint:disable */
/* eslint-disable */
import { StateDto } from './state-dto';
export interface AddressDto {
  city: string;
  id?: number;
  state?: StateDto;
  stateId: number;
  street: string;
  zipcode: string;
}
