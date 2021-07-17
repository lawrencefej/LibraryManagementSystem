import { CheckoutForListDto, LibraryCardForDetailedDto } from 'src/dto/models';

export interface ILibraryCardDetailResponse {
  card: LibraryCardForDetailedDto;
  checkouts: CheckoutForListDto[];
}
