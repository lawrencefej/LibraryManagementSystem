import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseLayoutComponent } from '../layout/base-layout/base-layout.component';
import { AuthGuard } from '../_guards/auth.guard';
import { LibraryCardDetailComponent } from './library-card-detail/library-card-detail.component';
import { LibraryCardDetailResolver } from './library-card-detail/library-card-detail.resolver';
import { LibraryCardListComponent } from './library-card-list/library-card-list.component';
import { LibraryCardListResolver } from './library-card-list/library-card-list.resolver';
import { LibraryCardCheckoutCanDeactivateGuardService } from './services/library-card-checkout-can-deactivate-guard.service';
import { LibraryCardEditCanDeactivateGuardService } from './services/library-card-edit-can-deactivate-guard.service';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'cards',
        component: LibraryCardListComponent,
        resolve: {
          initData: LibraryCardListResolver
        }
      },
      {
        path: 'cards/:id',
        component: LibraryCardDetailComponent,
        resolve: {
          initData: LibraryCardDetailResolver
        },
        canDeactivate: [LibraryCardEditCanDeactivateGuardService, LibraryCardCheckoutCanDeactivateGuardService]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LibraryCardRoutingModule {}
