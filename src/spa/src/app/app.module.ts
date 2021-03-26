import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AdminListResolver } from './_resolver/admin-list.resolver';
import { AdminService } from './_services/admin.service';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AssetDetailResolver } from './_resolver/asset-detail.resolver';
import { AssetListResolver } from './_resolver/asset-list.resolver';
import { AssetService } from './_services/asset.service';
import { AuthGuard } from './_guards/auth.guard';
import { AuthModule } from './auth/auth.module';
import { AuthService } from './_services/auth.service';
import { AuthorAssetResolver } from './_resolver/author-asset.resolver';
import { AuthorListResolver } from './_resolver/author-list.resolver';
import { AuthorService } from './_services/author.service';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { BasketService } from './_services/basket.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CheckoutDetailResolver } from './_resolver/checkout-detail.resolver';
import { CheckoutListResolver } from './_resolver/checkout-list.resolver';
import { CommonModule } from '@angular/common';
import { DashboardModule } from './dashboard/dashboard.module';
import { ErrorInterceptor } from './_services/error.interceptor';
import { JwtModule } from '@auth0/angular-jwt';
import { LayoutModule } from '@angular/cdk/layout';
import { LoaderInterceptor } from './_services/loader-interceptor';
import { LoaderService } from './_services/loader.service';
import { MainModule } from './main/main.module';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { MemberService } from './_services/member.service';
import { NgModule } from '@angular/core';
import { NotificationService } from './_services/notification.service';
import { PhotoService } from './_services/photo.service';
import { ReportService } from './_services/report.service';
import { ResponsiveNavComponent } from './core/responsive-nav/responsive-nav.component';
import { SharedModule } from './shared/shared.module';
import { UserProfileResolver } from './_resolver/user-profile.resolver';
import { UserService } from './_services/user.service';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  exports: [],
  declarations: [AppComponent, BaseLayoutComponent, ResponsiveNavComponent],
  imports: [
    AppRoutingModule,
    AuthModule,
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    DashboardModule,
    HttpClientModule,
    LayoutModule,
    MainModule,
    MatToolbarModule,
    MatIconModule,
    MatDividerModule,
    MatSidenavModule,
    MatMenuModule,
    MatCardModule,
    MatListModule,
    // ConfirmDialogComponent,
    // DashboardLayoutComponent,
    // DefaultLayoutComponent,
    // FooterComponent,
    // HasRoleDirective,
    // HeaderComponent,
    // LoginLayoutComponent,
    // NotFoundComponent,
    // PhoneNumberPipe,
    // PreventUnsavedComponent,
    // ProgressSpinnerComponent,
    // ServerErrorComponent,
    // SidebarComponent,
    SharedModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: ['localhost:5000'],
        disallowedRoutes: [],
      },
    }),
  ],
  providers: [
    AdminService,
    AdminListResolver,
    AssetDetailResolver,
    AssetListResolver,
    AssetService,
    AuthGuard,
    AuthorAssetResolver,
    AuthorListResolver,
    AuthorService,
    AuthService,
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
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  // entryComponents: [
  // ],
  bootstrap: [AppComponent],
})
export class AppModule {}
