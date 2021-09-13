import { Component, Input } from '@angular/core';

@Component({
  selector: 'lms-error-list',
  templateUrl: './error-list.component.html',
  styleUrls: ['./error-list.component.css']
})
export class ErrorListComponent {
  @Input()
  errors: string[] = [];

  constructor() {}
}
