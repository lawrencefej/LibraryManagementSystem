import { Injectable, OnDestroy } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { LibraryAssetForListDto } from 'src/dto/models';
import { CategoryService } from '../services/category.service';
import { LibraryAssetService } from '../services/library-asset.service';

@Injectable()
export class LibraryAssetListResolver implements Resolve<PaginatedResult<LibraryAssetForListDto[]>>, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  constructor(private readonly assetService: LibraryAssetService, private readonly categoryService: CategoryService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  resolve(): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    this.categoryService.categories$.pipe(takeUntil(this.unsubscribe)).subscribe();
    return this.assetService.getAssets(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '');
  }
}
