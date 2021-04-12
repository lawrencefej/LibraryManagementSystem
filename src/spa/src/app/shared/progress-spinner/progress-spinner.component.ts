import { Component, OnInit } from '@angular/core';

import { LoaderService } from 'src/app/_services/loader.service';
import { Subject } from 'rxjs/internal/Subject';

@Component({
  selector: 'app-progress-spinner',
  templateUrl: './progress-spinner.component.html',
  styleUrls: ['./progress-spinner.component.css']
})
export class ProgressSpinnerComponent implements OnInit {
  isLoading: Subject<boolean> = this.loaderService.isLoading;
  color = 'accent';
  mode = 'indeterminate';
  value = 50;

  constructor(private loaderService: LoaderService) { }

  ngOnInit() {
  }

}
