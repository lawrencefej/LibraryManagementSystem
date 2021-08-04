import { Injectable, OnDestroy } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { LibraryAssetForDetailedDto } from 'src/dto/models';
import { CategoryService } from '../services/category.service';
import { LibraryAssetService } from '../services/library-asset.service';

@Injectable()
export class LibraryAssetDetailResolver implements Resolve<LibraryAssetForDetailedDto>, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  constructor(private readonly assetService: LibraryAssetService, private readonly categoryService: CategoryService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  resolve(route: ActivatedRouteSnapshot): Observable<LibraryAssetForDetailedDto> {
    this.categoryService.categories$.pipe(takeUntil(this.unsubscribe)).subscribe();
    return this.assetService.getAsset(route.params.id);
  }
}
