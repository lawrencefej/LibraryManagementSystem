/* tslint:disable */
/* eslint-disable */
import { LibraryAssetForBasket } from './library-asset-for-basket';
export interface BasketForCheckoutDto {
  assets?: null | Array<LibraryAssetForBasket>;
  id?: number;
  libraryCardId?: number;
}
