import { BasketForCheckoutDto } from 'src/dto/models';
import { LibraryAssetForBasketViewModel } from './library-asset-for-basket-view-model';

export interface BasketViewModel extends BasketForCheckoutDto {
  cardNumber: string;
  photoUrl: string;
  assets: LibraryAssetForBasketViewModel[];
  active: boolean;
}
