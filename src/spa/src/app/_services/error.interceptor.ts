import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from './notification.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private readonly notify: NotificationService, private readonly router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const modelStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();
              } else if (typeof error.error === 'object') {
                if (error.error.length) {
                  throw error.error;
                } else {
                  this.notify.error(`${error.error.title} ${error.status}`);
                }
              } else {
                this.notify.error(`${error.status}, ${error.error}`);
              }
              break;
            case 401:
              this.notify.error('Unauthorized');
              break;
            case 403:
              this.notify.error('Forbiden');
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = { state: { error: error.error } };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.notify.error('Something unexpected went wrong');
              console.log(error);
              break;
          }
        }

        // TODO use route to error components for certain errors
        // if (error instanceof HttpErrorResponse) {
        //   if (error.status === (401 || 403)) {
        //     return throwError(error.statusText);
        //   }
        //   const applicationError = error.headers.get('Application-Error');
        //   if (applicationError) {
        //     console.error(applicationError);
        //     return throwError(applicationError);
        //   }

        //   const serverError = error.error;
        //   let modalStateErrors = '';
        //   if (serverError && typeof serverError === 'object') {
        //     for (const key in serverError) {
        //       if (serverError[key]) {
        //         modalStateErrors += serverError[key] + '\n';
        //       }
        //     }
        //   }
        //   return throwError(modalStateErrors || serverError || 'server Error');
        // }
        return throwError(error);
      })
    );
  }
}

export const ErrorinterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
