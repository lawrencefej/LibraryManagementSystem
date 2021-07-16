import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LibraryCardAdvancedSearchComponent } from './library-card-advanced-search/library-card-advanced-search.component';
import { LibraryCardDetailComponent } from './library-card-detail/library-card-detail.component';
import { LibraryCardListComponent } from './library-card-list/library-card-list.component';
import { LibraryCardListResolver } from './library-card-list/library-card-list.resolver';
import { LibraryCardSearchComponent } from './library-card-search/library-card-search.component';

const routes: Routes = [
  {
    path: 'home',
    component: LibraryCardSearchComponent
  },
  {
    path: 'cards',
    component: LibraryCardListComponent,
    resolve: { cards: LibraryCardListResolver }
  },
  {
    path: 'cards/:id',
    component: LibraryCardDetailComponent
  },
  {
    path: 'cards/:id',
    component: LibraryCardDetailComponent
  },
  {
    path: 'advanced-search',
    component: LibraryCardAdvancedSearchComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LibraryCardRoutingModule {}
