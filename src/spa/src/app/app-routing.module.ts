import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { DashboardPanelComponent } from './dashboard/dashboard-panel/dashboard-panel.component';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { UserProfileComponent } from './main/user/user-profile/user-profile.component';
import { LoginLayoutComponent } from './shared/layout/login-layout/login-layout.component';
import { AuthGuard } from './_guards/auth.guard';
import { IsDevGuard } from './_guards/is-dev.guard';
import { UserProfileResolver } from './_resolver/user-profile.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {
        path: 'library-card',
        loadChildren: () => import('./library-card/library-card.module').then(m => m.LibraryCardModule)
      },
      {
        path: 'checkout',
        loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule)
      },
      {
        path: 'admin',
        loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
      },
      {
        path: 'catalog',
        loadChildren: () => import('./library-asset/library-asset.module').then(m => m.LibraryAssetModule)
      },
      {
        path: 'test',
        loadChildren: () => import('./test/test.module').then(m => m.TesModule),
        canLoad: [IsDevGuard]
      },
      {
        path: 'authors',
        loadChildren: () => import('./author/author.module').then(m => m.AuthorModule)
      },
      {
        path: 'dashboard',
        component: DashboardPanelComponent,
        data: { allowedRoles: ['Admin'] }
      },
      {
        path: 'user-profile',
        component: UserProfileComponent,
        resolve: { user: UserProfileResolver }
      }
    ]
  },
  {
    path: '',
    component: LoginLayoutComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'forgot-password', component: ForgotPasswordComponent },
      { path: 'reset-password/:id/:code', component: ResetPasswordComponent }
    ]
  },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      relativeLinkResolution: 'legacy',
      scrollPositionRestoration: 'enabled',
      preloadingStrategy: PreloadAllModules
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}
