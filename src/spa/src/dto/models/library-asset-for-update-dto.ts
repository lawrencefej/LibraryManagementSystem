/* tslint:disable */
/* eslint-disable */
import { LibraryAssetAuthorDto } from './library-asset-author-dto';
import { LibraryAssetCategoryDto } from './library-asset-category-dto';
import { LibraryAssetTypeDto } from './library-asset-type-dto';
export interface LibraryAssetForUpdateDto {
  assetAuthors: Array<LibraryAssetAuthorDto>;
  assetCategories: Array<LibraryAssetCategoryDto>;
  assetType: LibraryAssetTypeDto;
  copiesAvailable: number;
  description: string;
  deweyIndex?: null | string;
  id: number;
  isbn?: null | string;
  numberOfCopies: number;
  title: string;
  year: number;
}
