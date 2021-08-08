import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';
import { LayoutModule } from './layout/layout.module';
import { SearchModule } from './search/search.module';
import { ErrorInterceptor } from './_services/error.interceptor';
import { LoaderInterceptor } from './_services/loader-interceptor';

export function tokenGetter(): string {
  return localStorage.getItem('token');
}

@NgModule({
  // declarations: [AppComponent, BaseLayoutComponent, ResponsiveNavComponent],
  declarations: [AppComponent],
  imports: [
    // TODO Fix wht this is called in the auth guard then remove
    AuthModule,
    // BasketModule,
    BrowserAnimationsModule,
    BrowserModule,
    CommonModule,
    // DashboardModule,
    HttpClientModule,
    // LayoutModule,
    // MainModule,
    // MatButtonModule,
    // MatCardModule,
    // MatDividerModule,
    LayoutModule,
    MatSnackBarModule,
    // MatFormFieldModule,
    // MatIconModule,
    // MatListModule,
    // MatMenuModule,
    // MatSidenavModule,
    // MatToolbarModule,
    SearchModule,
    // SharedModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: ['localhost:5000', 'localhost:5001'],
        disallowedRoutes: []
      }
    }),
    AppRoutingModule // Should be last, so that the wild card routes will be the last as well
  ],
  providers: [
    // AdminService,
    // AssetService,
    // // AuthGuard,
    // AuthService,
    // AuthorService,
    // LoaderService,
    // MemberService,
    // NotificationService,
    // PhotoService,
    // ReportService,
    // UserProfileResolver,
    // UserService,
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
