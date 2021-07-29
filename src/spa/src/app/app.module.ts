import { LayoutModule } from '@angular/cdk/layout';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';
import { ResponsiveNavComponent } from './core/responsive-nav/responsive-nav.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { MainModule } from './main/main.module';
import { SharedModule } from './shared/shared.module';
import { AuthGuard } from './_guards/auth.guard';
import { AdminListResolver } from './_resolver/admin-list.resolver';
import { AssetDetailResolver } from './_resolver/asset-detail.resolver';
import { AssetListResolver } from './_resolver/asset-list.resolver';
import { AuthorAssetResolver } from './_resolver/author-asset.resolver';
import { AuthorListResolver } from './_resolver/author-list.resolver';
import { CheckoutDetailResolver } from './_resolver/checkout-detail.resolver';
import { CheckoutListResolver } from './_resolver/checkout-list.resolver';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { UserProfileResolver } from './_resolver/user-profile.resolver';
import { AdminService } from './_services/admin.service';
import { AssetService } from './_services/asset.service';
import { AuthService } from './_services/auth.service';
import { AuthorService } from './_services/author.service';
import { BasketService } from './_services/basket.service';
import { ErrorInterceptor } from './_services/error.interceptor';
import { LoaderInterceptor } from './_services/loader-interceptor';
import { LoaderService } from './_services/loader.service';
import { MemberService } from './_services/member.service';
import { NotificationService } from './_services/notification.service';
import { PhotoService } from './_services/photo.service';
import { ReportService } from './_services/report.service';
import { UserService } from './_services/user.service';

export function tokenGetter(): string {
  return localStorage.getItem('token');
}

@NgModule({
  exports: [],
  declarations: [AppComponent, BaseLayoutComponent, ResponsiveNavComponent],
  imports: [
    AuthModule,
    BrowserAnimationsModule,
    BrowserModule,
    CommonModule,
    DashboardModule,
    // ErrorModule,
    HttpClientModule,
    LayoutModule,
    MainModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    SharedModule,
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
    AdminListResolver,
    AdminService,
    AssetDetailResolver,
    AssetListResolver,
    AssetService,
    AuthGuard,
    AuthService,
    AuthorAssetResolver,
    AuthorListResolver,
    AuthorService,
    BasketService,
    CheckoutDetailResolver,
    CheckoutListResolver,
    LoaderService,
    MemberDetailResolver,
    MemberListResolver,
    MemberService,
    NotificationService,
    PhotoService,
    ReportService,
    UserProfileResolver,
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  // entryComponents: [
  // ],
  bootstrap: [AppComponent]
})
export class AppModule {}
