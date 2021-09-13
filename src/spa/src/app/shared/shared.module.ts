import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { HasRoleDirective } from '../_directives/has-role.directive';
import { IsDevDirective } from '../_directives/is-dev.directive';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { DashboardLayoutComponent } from './layout/dashboard-layout/dashboard-layout.component';
import { DefaultLayoutComponent } from './layout/default-layout/default-layout.component';
import { FooterComponent } from './navigation/footer/footer.component';
import { HeaderComponent } from './navigation/header/header.component';
import { SidebarComponent } from './navigation/sidebar/sidebar.component';
import { PhoneNumberPipe } from './pipes/phone-number.pipe';
import { PreventUnsavedComponent } from './prevent-unsaved/prevent-unsaved.component';
import { ProgressSpinnerComponent } from './progress-spinner/progress-spinner.component';
import { CheckoutSharedService } from './services/checkout-shared.service';

@NgModule({
  exports: [
    ConfirmDialogComponent,
    DashboardLayoutComponent,
    DefaultLayoutComponent,
    FlexLayoutModule,
    FooterComponent,
    HasRoleDirective,
    HeaderComponent,
    IsDevDirective,
    PhoneNumberPipe,
    PreventUnsavedComponent,
    ProgressSpinnerComponent,
    SidebarComponent
  ],
  declarations: [
    ConfirmDialogComponent,
    DashboardLayoutComponent,
    DefaultLayoutComponent,
    FooterComponent,
    HasRoleDirective,
    HeaderComponent,
    IsDevDirective,
    PhoneNumberPipe,
    PreventUnsavedComponent,
    ProgressSpinnerComponent,
    SidebarComponent
  ],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatButtonModule,
    MatDialogModule,
    MatIconModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    ReactiveFormsModule,
    RouterModule
  ],
  providers: [CheckoutSharedService],
  entryComponents: [ConfirmDialogComponent, PreventUnsavedComponent]
})
export class SharedModule {}
