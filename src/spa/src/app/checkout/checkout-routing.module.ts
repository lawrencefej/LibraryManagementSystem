import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseLayoutComponent } from '../layout/base-layout/base-layout.component';
import { AuthGuard } from '../_guards/auth.guard';
import { CheckoutListComponent } from './checkout-list/checkout-list.component';
import { CheckoutListResolver } from './checkout-list/checkout-list.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'home',
        component: CheckoutListComponent,
        resolve: { initData: CheckoutListResolver }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CheckoutRoutingModule {}
