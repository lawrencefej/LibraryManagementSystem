import { PaginatedResult } from 'src/app/_models/pagination';
import { CheckoutForListDto, LibraryCardForDetailedDto, StateDto } from 'src/dto/models';

export interface ILibraryCardDetailResponse {
  card: LibraryCardForDetailedDto;
  checkouts: PaginatedResult<CheckoutForListDto[]>;
  states: StateDto[];
}
