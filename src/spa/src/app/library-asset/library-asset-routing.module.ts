import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseLayoutComponent } from '../layout/base-layout/base-layout.component';
import { AuthGuard } from '../_guards/auth.guard';
import { LibraryAssetDetailComponent } from './library-asset-detail/library-asset-detail.component';
import { LibraryAssetDetailResolver } from './library-asset-detail/library-asset-detail.resolver';
import { LibraryAssetListComponent } from './library-asset-list/library-asset-list.component';
import { LibraryAssetListResolver } from './library-asset-list/library-asset-list.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'home',
        component: LibraryAssetListComponent,
        resolve: {
          initData: LibraryAssetListResolver
        }
      },
      {
        path: 'detail/:id',
        component: LibraryAssetDetailComponent,
        resolve: {
          initData: LibraryAssetDetailResolver
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LibraryAssetRoutingModule {}
