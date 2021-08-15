import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';
import { SearchModule } from './search/search.module';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { ErrorInterceptor } from './_services/error.interceptor';
import { LoaderInterceptor } from './_services/loader-interceptor';

@NgModule({
  // declarations: [AppComponent, BaseLayoutComponent, ResponsiveNavComponent],
  declarations: [AppComponent],
  imports: [
    // TODO Fix wht this is called in the auth guard then remove
    // AuthModule,
    // BasketModule,
    BrowserAnimationsModule,
    BrowserModule,
    CommonModule,
    HttpClientModule,
    LayoutModule,
    MatSnackBarModule,
    SearchModule,
    AppRoutingModule // Should be last, so that the wild card routes will be the last as well
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
