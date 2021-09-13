import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatTabsModule } from '@angular/material/tabs';
import { SharedModule } from '../shared/shared.module';
import { LibraryCardAdvancedSearchComponent } from './search-library-card/library-card-advanced-search/library-card-advanced-search.component';
import { LibraryCardBasicSearchComponent } from './search-library-card/library-card-basic-search/library-card-basic-search.component';
import { SearchLibraryCardComponent } from './search-library-card/search-library-card.component';
import { SearchRoutingModule } from './search-routing.module';
import { SearchService } from './services/search.service';

@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatTabsModule,
    ReactiveFormsModule,
    SearchRoutingModule,
    SharedModule
  ],
  declarations: [LibraryCardAdvancedSearchComponent, LibraryCardBasicSearchComponent, SearchLibraryCardComponent],
  providers: [SearchService]
})
export class SearchModule {}
