import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { BasketModule } from '../basket/basket.module';
import { SharedModule } from '../shared/shared.module';
import { BaseLayoutComponent } from './base-layout/base-layout.component';
import { ResponsiveNavComponent } from './responsive-nav/responsive-nav.component';

@NgModule({
  declarations: [BaseLayoutComponent, ResponsiveNavComponent],
  imports: [
    RouterModule,
    BasketModule,
    SharedModule,
    MatButtonModule,
    CommonModule,
    MatSidenavModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    MatMenuModule,
    MatToolbarModule
  ]
})
export class LayoutModule {}
