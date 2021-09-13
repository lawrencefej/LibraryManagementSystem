import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ErrorModule } from '../error/error.module';
import { SharedModule } from '../shared/shared.module';
import { LibraryCardCheckoutComponent } from './library-card-checkout/library-card-checkout.component';
import { LibraryCardDetailCheckoutListComponent } from './library-card-detail/library-card-detail-checkout-list/library-card-detail-checkout-list.component';
import { LibraryCardDetailComponent } from './library-card-detail/library-card-detail.component';
import { LibraryCardDetailResolver } from './library-card-detail/library-card-detail.resolver';
import { LibraryCardEditComponent } from './library-card-detail/library-card-edit/library-card-edit.component';
import { LibraryCardListComponent } from './library-card-list/library-card-list.component';
import { LibraryCardListResolver } from './library-card-list/library-card-list.resolver';
import { LibraryCardRoutingModule } from './library-card-routing.module';
import { LibraryCardComponent } from './library-card/library-card.component';
import { LibraryCardCheckoutCanDeactivateGuardService } from './services/library-card-checkout-can-deactivate-guard.service';
import { LibraryCardEditCanDeactivateGuardService } from './services/library-card-edit-can-deactivate-guard.service';
import { LibraryCardService } from './services/library-card.service';

@NgModule({
  imports: [
    CommonModule,
    ErrorModule,
    FlexLayoutModule,
    LibraryCardRoutingModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatRadioModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatTooltipModule,
    ReactiveFormsModule,
    SharedModule
  ],
  declarations: [
    LibraryCardCheckoutComponent,
    LibraryCardComponent,
    LibraryCardDetailCheckoutListComponent,
    LibraryCardDetailComponent,
    LibraryCardEditComponent,
    LibraryCardListComponent
  ],
  providers: [
    LibraryCardCheckoutCanDeactivateGuardService,
    LibraryCardDetailResolver,
    LibraryCardEditCanDeactivateGuardService,
    LibraryCardListResolver,
    LibraryCardService
  ]
})
export class LibraryCardModule {}
