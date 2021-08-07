import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseLayoutComponent } from '../layout/base-layout/base-layout.component';
import { AuthGuard } from '../_guards/auth.guard';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AuthorDetailResolver } from './author-detail/author-detail.resolver';
import { AuthorListComponent } from './author-list/author-list.component';
import { AuthorListResolver } from './author-list/author-list.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'home',
        component: AuthorListComponent,
        resolve: {
          initData: AuthorListResolver
        }
      },
      {
        path: 'detail/:id',
        component: AuthorDetailComponent,
        resolve: {
          initData: AuthorDetailResolver
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorRoutingModule {}
