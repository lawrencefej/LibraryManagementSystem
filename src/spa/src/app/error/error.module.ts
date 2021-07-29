import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { ErrorListComponent } from './error-list/error-list.component';

@NgModule({
  declarations: [ErrorListComponent],
  imports: [CommonModule, MatListModule],
  exports: [ErrorListComponent]
})
export class ErrorModule {}
