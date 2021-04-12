import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-total-card',
  templateUrl: './total-card.component.html',
  styleUrls: ['./total-card.component.css']
})
export class TotalCardComponent {
  @Input() iconName: string;
  @Input() name: string;
  @Input() totalNumber: number;

  constructor() {}
}
