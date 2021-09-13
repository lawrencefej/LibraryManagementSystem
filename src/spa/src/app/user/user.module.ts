import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatRadioModule } from '@angular/material/radio';
import { MatTabsModule } from '@angular/material/tabs';
import { ErrorModule } from '../error/error.module';
import { SharedModule } from '../shared/shared.module';
import { UserProfileEditGuard } from './services/user-profile-edit.guard';
import { UserService } from './services/user.service';
import { UserPasswordChangeComponent } from './user-profile/user-password-change/user-password-change.component';
import { UserProfileEditComponent } from './user-profile/user-profile-edit/user-profile-edit.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileResolver } from './user-profile/user-profile.resolver';
import { UserRoutingModule } from './user-routing.module';

@NgModule({
  declarations: [UserProfileComponent, UserPasswordChangeComponent, UserProfileEditComponent],
  imports: [
    CommonModule,
    ErrorModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatFormFieldModule,
    MatInputModule,
    MatListModule,
    MatRadioModule,
    MatTabsModule,
    ReactiveFormsModule,
    SharedModule,
    UserRoutingModule
  ],
  providers: [UserProfileResolver, UserService, UserProfileEditGuard]
})
export class UserModule {}
