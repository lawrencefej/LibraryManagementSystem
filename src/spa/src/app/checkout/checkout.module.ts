import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CheckoutListComponent } from './checkout-list/checkout-list.component';
import { MatSelectModule } from '@angular/material/select';
import { CheckoutListResolver } from './checkout-list/checkout-list.resolver';

@NgModule({
  imports: [
    CheckoutRoutingModule,
    CommonModule,
    MatFormFieldModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    ReactiveFormsModule,
    SharedModule
  ],
  declarations: [CheckoutListComponent],
  providers: [CheckoutListResolver]
})
export class CheckoutModule {}
