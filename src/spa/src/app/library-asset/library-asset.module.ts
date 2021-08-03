import { DragDropModule } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { SharedModule } from '../shared/shared.module';
import { LibraryAssetDetailCheckoutListComponent } from './library-asset-detail/library-asset-detail-checkout-list/library-asset-detail-checkout-list.component';
import { LibraryAssetDetailComponent } from './library-asset-detail/library-asset-detail.component';
import { LibraryAssetDetailResolver } from './library-asset-detail/library-asset-detail.resolver';
import { LibraryAssetEditComponent } from './library-asset-detail/library-asset-edit/library-asset-edit.component';
import { LibraryAssetListComponent } from './library-asset-list/library-asset-list.component';
import { LibraryAssetListResolver } from './library-asset-list/library-asset-list.resolver';
import { LibraryAssetRoutingModule } from './library-asset-routing.module';
import { LibraryAssetComponent } from './library-asset/library-asset.component';
import { CategoryService } from './services/category.service';
import { LibraryAssetService } from './services/library-asset.service';

@NgModule({
  declarations: [
    LibraryAssetComponent,
    LibraryAssetDetailCheckoutListComponent,
    LibraryAssetDetailComponent,
    LibraryAssetEditComponent,
    LibraryAssetListComponent
  ],
  imports: [
    CommonModule,
    LibraryAssetRoutingModule,
    DragDropModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDialogModule,
    MatDividerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [LibraryAssetService, LibraryAssetListResolver, LibraryAssetDetailResolver, CategoryService]
})
export class LibraryAssetModule {}
