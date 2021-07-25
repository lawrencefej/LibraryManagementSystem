import { NgModule } from '@angular/core';
import { Routes, RouterModule, Router } from '@angular/router';
import { CheckoutListComponent } from './checkout-list/checkout-list.component';
import { CheckoutListResolver } from './checkout-list/checkout-list.resolver';

const routes: Routes = [
  { path: 'home', component: CheckoutListComponent, resolve: { initData: CheckoutListResolver } }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CheckoutRoutingModule {}
