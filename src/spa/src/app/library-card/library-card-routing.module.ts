import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LibraryCardAdvancedSearchComponent } from './library-card-advanced-search/library-card-advanced-search.component';
import { LibraryCardDetailComponent } from './library-card-detail/library-card-detail.component';
import { LibraryCardDetailResolver } from './library-card-detail/library-card-detail.resolver';
import { LibraryCardListComponent } from './library-card-list/library-card-list.component';
import { LibraryCardListResolver } from './library-card-list/library-card-list.resolver';
import { LibraryCardSearchComponent } from './library-card-search/library-card-search.component';
import { LibraryCardCheckoutCanDeactivateGuardService } from './services/library-card-checkout-can-deactivate-guard.service';
import { LibraryCardEditCanDeactivateGuardService } from './services/library-card-edit-can-deactivate-guard.service';

const routes: Routes = [
  {
    path: 'home',
    component: LibraryCardSearchComponent
  },
  {
    path: 'cards',
    component: LibraryCardListComponent,
    resolve: { initData: LibraryCardListResolver }
  },
  {
    path: 'cards/:id',
    component: LibraryCardDetailComponent,
    resolve: { initData: LibraryCardDetailResolver },
    canDeactivate: [LibraryCardEditCanDeactivateGuardService, LibraryCardCheckoutCanDeactivateGuardService]
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
