import { CheckoutForListDto, LibraryCardForDetailedDto, StateDto } from 'src/dto/models';

export interface ILibraryCardDetailResponse {
  card: LibraryCardForDetailedDto;
  checkouts: CheckoutForListDto[];
  states: StateDto[];
}
