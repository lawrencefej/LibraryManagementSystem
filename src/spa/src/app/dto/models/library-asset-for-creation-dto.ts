/* tslint:disable */
/* eslint-disable */
import { LibraryAssetAuthorDto } from './library-asset-author-dto';
import { LibraryAssetCategoryDto } from './library-asset-category-dto';
import { LibraryAssetTypeDto } from './library-asset-type-dto';
export interface LibraryAssetForCreationDto {
  assetAuthors?: null | Array<LibraryAssetAuthorDto>;
  assetCategories?: null | Array<LibraryAssetCategoryDto>;
  assetType?: LibraryAssetTypeDto;
  description?: null | string;
  deweyIndex?: null | string;
  isbn?: null | string;
  numberOfCopies?: number;
  title?: null | string;
  year?: number;
}
