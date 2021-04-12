import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';

import { AssetService } from '../_services/asset.service';
import { Injectable } from '@angular/core';
import { LibraryAsset } from '../_models/libraryAsset';
import { NotificationService } from '../_services/notification.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AssetListResolver implements Resolve<LibraryAsset[]> {
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private assetService: AssetService,
    private notify: NotificationService,
    private router: Router
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<LibraryAsset[]> {
    return this.assetService
      .getPaginatedAssets(this.pageNumber, this.pageSize, '', '', '')
      .pipe(
        catchError((error) => {
          this.notify.error('Problem retrieving data');
          this.router.navigate(['/memberSearch']);
          return of(null);
        })
      );
  }
}
