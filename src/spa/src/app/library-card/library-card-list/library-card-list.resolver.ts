import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { PaginatedResult } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { LibrarycardForListDto } from 'src/dto/models';
import { LibraryCardService } from '../services/library-card.service';

@Injectable()
export class LibraryCardListResolver implements Resolve<PaginatedResult<LibrarycardForListDto[]>> {
  constructor(private readonly cardService: LibraryCardService) {}

  resolve(): Observable<PaginatedResult<LibrarycardForListDto[]>> {
    return this.cardService.getCards(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '');
  }
}
