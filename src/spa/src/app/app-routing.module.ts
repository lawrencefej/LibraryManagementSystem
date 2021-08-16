import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { IsDevGuard } from './_guards/is-dev.guard';

const routes: Routes = [
  // {
  //   path: '',
  //   component: BaseLayoutComponent,
  //   runGuardsAndResolvers: 'always',
  //   canActivate: [AuthGuard],
  //   children: [
  //     {
  //       path: 'user-profile',
  //       component: UserProfileComponent,
  //       resolve: { user: UserProfileResolver }
  //     }
  //   ]
  // },
  // {
  //   path: '',
  //   component: SearchLibraryCardComponent
  // },
  // TODO Fix no path route issues
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'authors',
    loadChildren: () => import('./author/author.module').then(m => m.AuthorModule)
  },
  {
    path: 'checkout',
    loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule)
  },
  {
    path: 'catalog',
    loadChildren: () => import('./library-asset/library-asset.module').then(m => m.LibraryAssetModule)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'library-card',
    loadChildren: () => import('./library-card/library-card.module').then(m => m.LibraryCardModule)
  },
  {
    path: 'test',
    loadChildren: () => import('./test/test.module').then(m => m.TesModule),
    canLoad: [IsDevGuard]
  },
  // {
  //   path: '',
  //   redirectTo: 'home',
  //   pathMatch: 'full'
  // },
  {
    path: '**',
    redirectTo: 'home'
    // pathMatch: 'full'
  }
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
