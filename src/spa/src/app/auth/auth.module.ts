import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login/login.component';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgModule } from '@angular/core';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    ForgotPasswordComponent,
    LoginComponent,
    ResetPasswordComponent,
  ],
  imports: [SharedModule, MatCardModule, MatFormFieldModule, MatInputModule],
  exports: [ForgotPasswordComponent, LoginComponent, ResetPasswordComponent],
})
export class AuthModule {}
