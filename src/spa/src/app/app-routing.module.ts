import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { DashboardPanelComponent } from './dashboard/dashboard-panel/dashboard-panel.component';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { AssetDetailComponent } from './main/libraryAssets/asset-detail/asset-detail.component';
import { AssetListComponent } from './main/libraryAssets/asset-list/asset-list.component';
import { MemberAdvancedSearchComponent } from './main/member/member-advanced-search/member-advanced-search.component';
import { MemberDetailComponent } from './main/member/member-detail/member-detail.component';
import { MemberListComponent } from './main/member/member-list/member-list.component';
import { MemberSearchComponent } from './main/member/member-search/member-search.component';
import { UserProfileComponent } from './main/user/user-profile/user-profile.component';
import { LoginLayoutComponent } from './shared/layout/login-layout/login-layout.component';
import { AuthGuard } from './_guards/auth.guard';
import { IsDevGuard } from './_guards/is-dev.guard';
import { AssetDetailResolver } from './_resolver/asset-detail.resolver';
import { AssetListResolver } from './_resolver/asset-list.resolver';
import { MemberDetailResolver } from './_resolver/member-detail.resolver';
import { MemberListResolver } from './_resolver/member-list.resolver';
import { UserProfileResolver } from './_resolver/user-profile.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      // { path: '', redirectTo: 'member-search', pathMatch: 'full' },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      {
        path: 'library-card',
        loadChildren: () => import('./library-card/library-card.module').then(m => m.LibraryCardModule)
      },
      // {
      //   path: 'home',
      //   component: SearchLibraryCardComponent
      // },
      {
        path: 'checkout',
        loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule)
      },
      // TODO remove when the Library-card Module is complete
      { path: 'member-search', component: MemberSearchComponent },
      { path: 'advanced-search', component: MemberAdvancedSearchComponent },
      {
        // TODO remove when the Library-asset Module is complete
        path: 'catalog-old',
        component: AssetListComponent,
        resolve: { assets: AssetListResolver }
      },
      {
        path: 'catalog-old/:id',
        component: AssetDetailComponent,
        resolve: { asset: AssetDetailResolver }
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
        path: 'members',
        component: MemberListComponent,
        data: { allowedRoles: ['Admin', 'Librarian'] },
        resolve: { members: MemberListResolver }
      },
      {
        path: 'members/:id',
        component: MemberDetailComponent,
        data: { allowedRoles: ['Admin', 'Librarian'] },
        resolve: { member: MemberDetailResolver }
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
