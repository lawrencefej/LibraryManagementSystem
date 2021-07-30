import { LibraryCardForDetailedDto, StateDto } from 'src/dto/models';

export interface ILibraryCardDetailResponse {
  card: LibraryCardForDetailedDto;
  states: StateDto[];
}
