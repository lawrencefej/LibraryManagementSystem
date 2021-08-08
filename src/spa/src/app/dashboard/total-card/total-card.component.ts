import { Component, Input } from '@angular/core';

@Component({
  selector: 'lms-total-card',
  templateUrl: './total-card.component.html',
  styleUrls: ['./total-card.component.css']
})
export class TotalCardComponent {
  @Input() iconName!: string;
  @Input() name!: string;
  @Input() totalNumber!: number;

  constructor() {}
}
