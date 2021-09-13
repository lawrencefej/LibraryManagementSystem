/* tslint:disable */
/* eslint-disable */
import { LibraryAssetForListDto } from './library-asset-for-list-dto';
import { LibraryCardForDetailedDto } from './library-card-for-detailed-dto';
export interface CheckoutForDetailedDto {
  checkoutDate?: string;
  dateReturned?: null | string;
  dueDate?: string;
  id?: number;
  libraryAsset?: LibraryAssetForListDto;
  libraryCard?: LibraryCardForDetailedDto;
  status?: null | string;
}
