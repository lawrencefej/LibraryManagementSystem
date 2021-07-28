import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { UserRoles } from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'lms-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();
  private readonly baseUrl = environment.apiUrl + 'error/';

  validationErrors: string[] = [];
  userRoles = UserRoles;
  validationMessages = validationMessages;

  userForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl(''),
    email: new FormControl(''),
    role: new FormControl('')
  });

  constructor(private readonly http: HttpClient) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {}

  get404Error(): void {
    this.http
      .get(this.baseUrl + 'not-found')
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.log(error);
        }
      );
  }

  get400Error(): void {
    this.http
      .get(this.baseUrl + 'bad-request')
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.log(error);
        }
      );
  }

  get500Error(): void {
    this.http
      .get(this.baseUrl + 'server-error')
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.log(error);
        }
      );
  }

  get401Error(): void {
    this.http
      .get(this.baseUrl + 'auth')
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.log(error);
        }
      );
  }

  get400FluentValidation(): void {
    this.http
      .post(this.baseUrl + 'fluent-validation', {})
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.log(error);
          this.validationErrors = error;
        }
      );
  }

  get400DotNetValidation(): void {
    this.http
      .post(this.baseUrl + 'annotations', {})
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        response => {
          console.log(response);
        },
        error => {
          console.log(error);
          this.validationErrors = error;
        }
      );
  }
}
