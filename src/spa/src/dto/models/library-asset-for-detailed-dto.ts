/* tslint:disable */
/* eslint-disable */
import { AuthorDto } from './author-dto';
import { CategoryDto } from './category-dto';
export interface LibraryAssetForDetailedDto {
  added?: string;
  assetType: string;
  authors: Array<AuthorDto>;
  categories: Array<CategoryDto>;
  copiesAvailable: number;
  description?: null | string;
  deweyIndex?: null | string;
  id: number;
  isbn?: null | string;
  numberOfCopies: number;
  photoUrl: string;
  status: string;
  title: string;
  year: number;
}
