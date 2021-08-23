import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BaseLayoutComponent } from '../layout/base-layout/base-layout.component';
import { AuthGuard } from '../_guards/auth.guard';
import { UserProfileEditGuard } from './services/user-profile-edit.guard';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileResolver } from './user-profile/user-profile.resolver';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'profile',
        component: UserProfileComponent,
        resolve: {
          initData: UserProfileResolver
        },
        canDeactivate: [UserProfileEditGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule {}
