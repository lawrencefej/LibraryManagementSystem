import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from '../app-routing.module';
import { CommonModule } from '@angular/common';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { DashboardLayoutComponent } from './layout/dashboard-layout/dashboard-layout.component';
import { DefaultLayoutComponent } from './layout/default-layout/default-layout.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FooterComponent } from './navigation/footer/footer.component';
import { HasRoleDirective } from '../_directives/has-role.directive';
import { HeaderComponent } from './navigation/header/header.component';
import { LoginLayoutComponent } from './layout/login-layout/login-layout.component';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NgModule } from '@angular/core';
import { NotFoundComponent } from './error-pages/not-found/not-found.component';
import { PhoneNumberPipe } from './pipes/phone-number.pipe';
import { PreventUnsavedComponent } from './prevent-unsaved/prevent-unsaved.component';
import { ProgressSpinnerComponent } from './progress-spinner/progress-spinner.component';
import { RouterModule } from '@angular/router';
import { ServerErrorComponent } from './error-pages/server-error/server-error.component';
import { SidebarComponent } from './navigation/sidebar/sidebar.component';

@NgModule({
  exports: [
    AppRoutingModule,
    CommonModule,
    ConfirmDialogComponent,
    DashboardLayoutComponent,
    DefaultLayoutComponent,
    FlexLayoutModule,
    FooterComponent,
    FormsModule,
    HasRoleDirective,
    HeaderComponent,
    LoginLayoutComponent,
    // MaterialModule,
    NotFoundComponent,
    PhoneNumberPipe,
    PreventUnsavedComponent,
    ProgressSpinnerComponent,
    ReactiveFormsModule,
    ServerErrorComponent,
    SidebarComponent,
  ],
  declarations: [
    ConfirmDialogComponent,
    DashboardLayoutComponent,
    DefaultLayoutComponent,
    FooterComponent,
    HasRoleDirective,
    HeaderComponent,
    LoginLayoutComponent,
    NotFoundComponent,
    PhoneNumberPipe,
    PreventUnsavedComponent,
    ProgressSpinnerComponent,
    ServerErrorComponent,
    SidebarComponent,
  ],
  imports: [
    AppRoutingModule,
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    // MyMaterialModule,
    ReactiveFormsModule,
    RouterModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatListModule,
    MatToolbarModule,
    MatDialogModule,
  ],
  entryComponents: [ConfirmDialogComponent, PreventUnsavedComponent],
})
export class SharedModule {}
