import { AssetType } from './assetType';
import { Author } from './author';
import { Category } from './category';
import { Photo } from './photo';

export interface LibraryAsset {
  id?: number;
  title?: string;
  year?: number;
  status?: string;
  cost?: number;
  added?: Date;
  copiesAvailable?: number;
  numberOfCopies?: number;
  description?: string;
  photoUrl?: string;
  photo?: Photo;
  assetTypeName?: string;
  assetType?: AssetType;
  assetTypeId?: number;
  authorName?: string;
  isbn?: string;
  deweyIndex?: string;
  category?: Category;
  categoryId?: number;
  authorId?: number;
  author?: Author;
}
