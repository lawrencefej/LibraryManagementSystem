import { Injectable } from '@angular/core';
import { CanLoad } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class IsDevGuard implements CanLoad {
  canLoad(): boolean {
    if (environment.production) {
      return false;
    } else {
      return true;
    }
  }
}
