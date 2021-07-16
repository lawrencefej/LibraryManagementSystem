/* tslint:disable */
/* eslint-disable */
import { Author } from './author';
import { LibraryAsset } from './library-asset';
export interface LibraryAssetAuthor {
  author?: Author;
  authorId?: number;
  libraryAsset?: LibraryAsset;
  librayAssetId?: number;
  order?: number;
}
