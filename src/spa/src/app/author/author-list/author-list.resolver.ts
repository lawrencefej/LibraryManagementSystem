import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { PaginatedResult } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { AuthorDto } from 'src/dto/models';
import { AuthorService } from '../services/author.service';

@Injectable()
export class AuthorListResolver implements Resolve<PaginatedResult<AuthorDto[]>> {
  constructor(private readonly authorService: AuthorService) {}

  resolve(): Observable<PaginatedResult<AuthorDto[]>> {
    return this.authorService.getAuthors(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '');
  }
}
