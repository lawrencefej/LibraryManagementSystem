import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { ErrorModule } from '../error/error.module';
import { SharedModule } from '../shared/shared.module';
import { AuthorAssetListComponent } from './author-detail/author-asset-list/author-asset-list.component';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AuthorDetailResolver } from './author-detail/author-detail.resolver';
import { AuthorListComponent } from './author-list/author-list.component';
import { AuthorListResolver } from './author-list/author-list.resolver';
import { AuthorRoutingModule } from './author-routing.module';
import { AuthorComponent } from './author/author.component';
import { AuthorService } from './services/author.service';

@NgModule({
  declarations: [AuthorAssetListComponent, AuthorComponent, AuthorDetailComponent, AuthorListComponent],
  imports: [
    AuthorRoutingModule,
    CommonModule,
    ErrorModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [AuthorService, AuthorDetailResolver, AuthorListResolver]
})
export class AuthorModule {}
