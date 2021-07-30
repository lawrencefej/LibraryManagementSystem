import { Component } from '@angular/core';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { Subject } from 'rxjs/internal/Subject';
import { LoaderService } from 'src/app/_services/loader.service';

@Component({
  selector: 'lms-progress-spinner',
  templateUrl: './progress-spinner.component.html',
  styleUrls: ['./progress-spinner.component.css']
})
export class ProgressSpinnerComponent {
  isLoading: Subject<boolean> = this.loaderService.isLoading;
  color = 'accent';
  mode = 'indeterminate' as ProgressSpinnerMode;
  value = 50;

  constructor(private loaderService: LoaderService) {}
}
