import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { ErrorModule } from '../error/error.module';
import { BasketComponent } from './basket/basket.component';
import { BasketService } from './services/basket.service';

@NgModule({
  declarations: [BasketComponent],
  imports: [
    CommonModule,
    ErrorModule,
    FlexLayoutModule,
    MatBadgeModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    MatMenuModule
  ],
  providers: [BasketService],
  exports: [BasketComponent]
})
export class BasketModule {}
