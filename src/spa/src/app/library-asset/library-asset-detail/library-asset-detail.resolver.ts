import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { LibraryAssetForDetailedDto } from 'src/dto/models';
import { LibraryAssetService } from '../services/library-asset.service';

@Injectable()
export class LibraryAssetDetailResolver implements Resolve<LibraryAssetForDetailedDto> {
  constructor(private readonly assetService: LibraryAssetService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<LibraryAssetForDetailedDto> {
    return this.assetService.getAsset(route.params.id);
  }
}
