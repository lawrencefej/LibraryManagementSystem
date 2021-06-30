import { Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AssetService } from '../_services/asset.service';
import { Injectable } from '@angular/core';
import { LibraryAsset } from '../_models/libraryAsset';
import { NotificationService } from '../_services/notification.service';
import { catchError } from 'rxjs/operators';
import { lmsResolverContants } from './resolver.constants';

@Injectable()
export class AssetListResolver implements Resolve<LibraryAsset[]> {
  constructor(
    private assetService: AssetService,
    private notify: NotificationService,
    private router: Router
  ) {}

  resolve(): Observable<LibraryAsset[]> {
    return this.assetService
      .getPaginatedAssets(
        lmsResolverContants.pageNumber,
        lmsResolverContants.pageSize,
        '',
        '',
        ''
      )
      .pipe(
        catchError(() => {
          this.notify.error('Problem retrieving data');
          this.router.navigate(['/memberSearch']);
          return of(null);
        })
      );
  }
}
