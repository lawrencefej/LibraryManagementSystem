import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';

import { AssetService } from '../_services/asset.service';
import { Injectable } from '@angular/core';
import { LibraryAsset } from '../_models/libraryAsset';
import { NotificationService } from '../_services/notification.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class AssetDetailResolver implements Resolve<LibraryAsset> {
  constructor(
    private assetService: AssetService,
    private router: Router,
    private notify: NotificationService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<LibraryAsset> {
    return this.assetService.getAsset(route.params.id).pipe(
      catchError((error) => {
        this.notify.error('Problem retrieving data');
        this.router.navigate(['/catalog']);
        return of(null);
      })
    );
  }
}
