import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminListComponent } from './admin-list/admin-list.component';
import { AdminListResolver } from './admin-list/admin-list.resolver';

const routes: Routes = [
  {
    path: 'home',
    component: AdminListComponent,
    resolve: { initData: AdminListResolver }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
