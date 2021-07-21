import { AdminComponent } from './admin/admin/admin.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { AssetCheckoutComponent } from './libraryAssets/asset-checkout/asset-checkout.component';
import { AssetComponent } from './libraryAssets/asset/asset.component';
import { AssetDetailComponent } from './libraryAssets/asset-detail/asset-detail.component';
import { AssetListComponent } from './libraryAssets/asset-list/asset-list.component';
import { AuthorAssetComponent } from './author/author-asset/author-asset.component';
import { AuthorComponent } from './author/author/author.component';
import { AuthorListComponent } from './author/author-list/author-list.component';
import { BasketComponent } from './basket/basket/basket.component';
import { BasketDetailComponent } from './basket/basket-detail/basket-detail.component';
import { CheckoutAssetComponent } from './checkout/checkout-asset/checkout-asset.component';
import { CheckoutListComponent } from './checkout/checkout-list/checkout-list.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatNativeDateModule } from '@angular/material/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MemberAdvancedSearchComponent } from './member/member-advanced-search/member-advanced-search.component';
import { MemberCheckoutsComponent } from './member/member-checkouts/member-checkouts.component';
import { MemberComponent } from './member/member/member.component';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';
import { MemberListComponent } from './member/member-list/member-list.component';
import { MemberSearchComponent } from './member/member-search/member-search.component';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { UserProfileComponent } from './user/user-profile/user-profile.component';
import { UserProfileEditComponent } from './user/user-profile-edit/user-profile-edit.component';
import { RouterModule } from '@angular/router';

@NgModule({
  exports: [BasketComponent, BasketDetailComponent],
  declarations: [
    AdminComponent,
    AdminPanelComponent,
    AssetCheckoutComponent,
    AssetComponent,
    AssetDetailComponent,
    AssetListComponent,
    AuthorAssetComponent,
    AuthorComponent,
    AuthorListComponent,
    BasketComponent,
    BasketDetailComponent,
    CheckoutAssetComponent,
    CheckoutListComponent,
    MemberAdvancedSearchComponent,
    MemberCheckoutsComponent,
    MemberComponent,
    MemberDetailComponent,
    MemberListComponent,
    MemberSearchComponent,
    UserProfileComponent,
    UserProfileEditComponent
  ],
  imports: [
    MatAutocompleteModule,
    MatAutocompleteModule,
    MatBadgeModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatSelectModule,
    MatSidenavModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    RouterModule,
    SharedModule
    // ReactiveFormsModule,
    // CommonModule,
  ]
  // entryComponents: [
  //   AdminComponent,
  //   AssetComponent,
  //   AuthorComponent,
  //   MemberComponent,
  //   UserProfileEditComponent]
})
export class MainModule {}
