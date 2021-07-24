import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LibraryCardComponent } from './library-card/library-card.component';
import { LibraryCardAdvancedSearchComponent } from './library-card-advanced-search/library-card-advanced-search.component';
import { LibraryCardListComponent } from './library-card-list/library-card-list.component';
import { LibraryCardSearchComponent } from './library-card-search/library-card-search.component';
import { LibraryCardService } from './services/library-card.service';
import { LibraryCardRoutingModule } from './library-card-routing.module';
import { LibraryCardDetailComponent } from './library-card-detail/library-card-detail.component';
import { SharedModule } from '../shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { LibraryCardListResolver } from './library-card-list/library-card-list.resolver';
import { MatSortModule } from '@angular/material/sort';
import { LibraryCardDetailResolver } from './library-card-detail/library-card-detail.resolver';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { LibraryCardEditComponent } from './library-card-edit/library-card-edit.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatRadioModule } from '@angular/material/radio';
import { LibraryCardCheckoutComponent } from './library-card-checkout/library-card-checkout.component';
import { LibraryCardEditCanDeactivateGuardService } from './services/library-card-edit-can-deactivate-guard.service';
import { LibraryCardCheckoutCanDeactivateGuardService } from './services/library-card-checkout-can-deactivate-guard.service';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { LibraryCardDetailCheckoutListComponent } from './library-card-detail/library-card-detail-checkout-list/library-card-detail-checkout-list.component';

@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    FormsModule,
    LibraryCardRoutingModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
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
    LibraryCardAdvancedSearchComponent,
    LibraryCardCheckoutComponent,
    LibraryCardComponent,
    LibraryCardDetailComponent,
    LibraryCardDetailCheckoutListComponent,
    LibraryCardEditComponent,
    LibraryCardListComponent,
    LibraryCardSearchComponent
  ],
  providers: [
    LibraryCardDetailResolver,
    LibraryCardCheckoutCanDeactivateGuardService,
    LibraryCardEditCanDeactivateGuardService,
    LibraryCardListResolver,
    LibraryCardService
  ]
})
export class LibraryCardModule {}
