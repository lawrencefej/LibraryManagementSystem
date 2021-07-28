import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { NotificationService } from 'src/app/_services/notification.service';

@Component({
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  resetForm: FormGroup;

  validationMessages = {
    email: [
      { type: 'required', message: 'Email is required' },
      { type: 'email', message: 'Please enter a valid Email' }
    ]
  };

  constructor(
    public authService: AuthService,
    private notify: NotificationService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  // tslint:disable-next-line: typedef
  ngOnInit() {
    this.createResetForm();
  }

  // tslint:disable-next-line: typedef
  createResetForm() {
    this.resetForm = this.fb.group({
      email: new FormControl('', Validators.compose([Validators.required, Validators.email]))
    });
  }

  // tslint:disable-next-line: typedef
  onSubmit() {
    this.authService.sendForgotPasswordLink(this.resetForm.value).subscribe(
      () => {
        this.notify.success('Please check your inbox for the reset link');
        this.resetForm.reset();
        this.router.navigate(['/login']);
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
