import { LibraryAsset } from './libraryAsset';

export interface Checkout {
  id?: number;
  title?: string;
  since?: Date;
  checkoutDate?: Date;
  dueDate?: Date;
  returnDate?: Date;
  until?: Date;
  userId?: number;
  libraryAssetId?: number;
  libraryCardId?: number;
  dateReturned?: Date;
  status?: string;
  statusId?: number | undefined;
  libraryCardNumber?: number;
  cardNumber?: string;
  assets?: LibraryAsset[];
  asset?: LibraryAsset;
}
