import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MustMatch } from 'src/app/shared/validators/password-match.validator';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { AuthenticationService } from 'src/app/_services/authentication.service';

@Component({
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  resetPasswordForm!: FormGroup;
  validationMessages = validationMessages;

  constructor(
    private readonly authService: AuthenticationService,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.createResetPasswordForm();
  }

  createResetPasswordForm(): void {
    this.resetPasswordForm = this.fb.group(
      {
        userId: new FormControl(this.route.snapshot.params.id),
        code: new FormControl(this.route.snapshot.params.code),
        password: new FormControl('', Validators.compose([Validators.required])),
        confirmPassword: new FormControl('', [Validators.required])
      },
      {
        validators: MustMatch('password', 'confirmPassword')
      }
    );
  }

  onSubmit(): void {
    this.authService.resetPassword(this.resetPasswordForm.value).subscribe(() => {
      this.notify.success('Password has been reset successfully');
      this.resetPasswordForm.reset();
      this.router.navigate(['/login']);
    });
  }
}
