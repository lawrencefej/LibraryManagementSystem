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

@NgModule({
  imports: [
    CommonModule,
    LibraryCardRoutingModule,
    SharedModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatExpansionModule,
    MatFormFieldModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule
  ],
  declarations: [
    LibraryCardAdvancedSearchComponent,
    LibraryCardComponent,
    LibraryCardDetailComponent,
    LibraryCardListComponent,
    LibraryCardSearchComponent
  ],
  providers: [LibraryCardService]
})
export class LibraryCardModule {}
