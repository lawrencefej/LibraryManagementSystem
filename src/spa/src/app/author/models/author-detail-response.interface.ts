import { PaginatedResult } from 'src/app/_models/pagination';
import { AuthorDto, LibraryAssetForListDto } from 'src/dto/models';

export interface AuthorDetailResponse {
  author: AuthorDto;
  assets: PaginatedResult<LibraryAssetForListDto[]>;
}
