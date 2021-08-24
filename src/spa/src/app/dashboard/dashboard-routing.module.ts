import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseLayoutComponent } from '../layout/base-layout/base-layout.component';
import { AdminGuard } from '../_guards/admin.guard';
import { AuthGuard } from '../_guards/auth.guard';
import { DashboardPanelComponent } from './dashboard-panel/dashboard-panel.component';
import { DashboardPanelResolver } from './dashboard-panel/dashboard-panel.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard, AdminGuard],
    children: [
      {
        path: 'home',
        component: DashboardPanelComponent,
        resolve: {
          initData: DashboardPanelResolver
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule {}
