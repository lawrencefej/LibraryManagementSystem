import { LibraryAssetForBasket } from 'src/dto/models';

export interface LibraryAssetForBasketViewModel extends LibraryAssetForBasket {
  title: string;
  photoUrl?: string;
  author: string;
}
