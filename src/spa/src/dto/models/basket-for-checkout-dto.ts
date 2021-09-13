/* tslint:disable */
/* eslint-disable */
import { LibraryAssetForBasket } from './library-asset-for-basket';
export interface BasketForCheckoutDto {
  assets: Array<LibraryAssetForBasket>;
  libraryCardId: number;
}
