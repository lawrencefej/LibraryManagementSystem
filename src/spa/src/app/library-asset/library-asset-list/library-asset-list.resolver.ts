import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { PaginatedResult } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { LibraryAssetForListDto } from 'src/dto/models';
import { LibraryAssetService } from '../services/library-asset.service';

@Injectable()
export class LibraryAssetListResolver implements Resolve<PaginatedResult<LibraryAssetForListDto[]>> {
  constructor(private readonly assetService: LibraryAssetService) {}

  resolve(): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    return this.assetService.getAssets(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '');
  }
}
