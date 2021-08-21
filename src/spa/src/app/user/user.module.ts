import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { UserProfileComponent } from './user-profile/user-profile.component';

@NgModule({
  declarations: [UserProfileComponent],
  imports: [CommonModule, UserRoutingModule]
})
export class UserModule {}
