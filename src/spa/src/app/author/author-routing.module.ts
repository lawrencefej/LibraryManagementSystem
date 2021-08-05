import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AuthorDetailResolver } from './author-detail/author-detail.resolver';
import { AuthorListComponent } from './author-list/author-list.component';
import { AuthorListResolver } from './author-list/author-list.resolver';

const routes: Routes = [
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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorRoutingModule {}
