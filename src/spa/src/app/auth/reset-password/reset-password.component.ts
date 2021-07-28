import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmPasswordValidator } from 'src/app/shared/validators/password-match.validator';
import { AuthService } from 'src/app/_services/auth.service';
import { NotificationService } from 'src/app/_services/notification.service';

@Component({
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;

  validationMessages = {
    password: [
      { type: 'required', message: 'Password is required' },
      { type: 'minlength', message: 'Password must be at least 4 characters long' }
    ],
    confirmPassword: [
      { type: 'required', message: 'Confirm password is required' },
      { type: 'confirmPasswordMatch', message: 'Password and Confirm Password do not match' }
    ]
  };

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private notify: NotificationService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.createResetPasswordForm();
  }

  createResetPasswordForm(): void {
    this.resetPasswordForm = this.fb.group(
      {
        userID: new FormControl(this.route.snapshot.params.id),
        code: new FormControl(this.route.snapshot.params.code),
        password: new FormControl('', Validators.compose([Validators.required])),
        confirmPassword: new FormControl('', [Validators.required])
      },
      {
        validator: ConfirmPasswordValidator
      }
    );
  }

  onSubmit(): void {
    this.authService.resetPassword(this.resetPasswordForm.value).subscribe(
      () => {
        this.notify.success('Password has been reset successfully');
        this.resetPasswordForm.reset();
        this.router.navigate(['/login']);
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
