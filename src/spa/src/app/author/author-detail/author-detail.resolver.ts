import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { AuthorDetailResponse } from '../models/author-detail-response.interface';
import { AuthorService } from '../services/author.service';

@Injectable()
export class AuthorDetailResolver implements Resolve<AuthorDetailResponse> {
  constructor(private readonly authorService: AuthorService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<AuthorDetailResponse> {
    return forkJoin({
      author: this.authorService.getAuthor(route.params.id),
      assets: this.authorService.getAssetsForAuthors(
        route.params.id,
        lmsResolverContants.pageNumber,
        lmsResolverContants.pageSize,
        '',
        '',
        ''
      )
    });
  }
}
