/* tslint:disable */
/* eslint-disable */
import { AuthorDto } from './author-dto';
import { CategoryDto } from './category-dto';
export interface LibraryAssetForDetailedDto {
  added?: string;
  assetType?: null | string;
  authors?: null | Array<AuthorDto>;
  categories?: null | Array<CategoryDto>;
  copiesAvailable?: number;
  description?: null | string;
  deweyIndex?: null | string;
  id?: number;
  isbn?: null | string;
  numberOfCopies?: number;
  photoUrl?: null | string;
  status?: null | string;
  title?: null | string;
  year?: number;
}
