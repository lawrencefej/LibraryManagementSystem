/* tslint:disable */
/* eslint-disable */
import { LibraryAssetAuthorDto } from './library-asset-author-dto';
import { LibraryAssetCategoryDto } from './library-asset-category-dto';
import { LibraryAssetTypeDto } from './library-asset-type-dto';
export interface LibraryAssetForCreationDto {
  assetAuthors: Array<LibraryAssetAuthorDto>;
  assetCategories: Array<LibraryAssetCategoryDto>;
  assetType?: LibraryAssetTypeDto;
  description?: null | string;
  deweyIndex?: null | string;
  isbn?: null | string;
  numberOfCopies?: number;
  title: string;
  year: number;
}
