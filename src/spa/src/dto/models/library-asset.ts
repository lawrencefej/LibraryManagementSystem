/* tslint:disable */
/* eslint-disable */
import { AssetPhoto } from './asset-photo';
import { LibraryAssetAuthor } from './library-asset-author';
import { LibraryAssetCategory } from './library-asset-category';
import { LibraryAssetStatus } from './library-asset-status';
import { LibraryAssetType } from './library-asset-type';
export interface LibraryAsset {
  added?: string;
  assetAuthors?: null | Array<LibraryAssetAuthor>;
  assetCategories?: null | Array<LibraryAssetCategory>;
  assetType?: LibraryAssetType;
  copiesAvailable?: number;
  description?: null | string;
  deweyIndex?: null | string;
  id?: number;
  isbn?: null | string;
  numberOfCopies?: number;
  photo?: AssetPhoto;
  status?: LibraryAssetStatus;
  title?: null | string;
  year?: number;
}
