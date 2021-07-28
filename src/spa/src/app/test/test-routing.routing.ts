import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestErrorsComponent } from './test-errors/test-errors.component';

const routes: Routes = [
  {
    path: 'error',
    component: TestErrorsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TestRoutingModule {}
