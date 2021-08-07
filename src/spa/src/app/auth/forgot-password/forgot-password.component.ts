import { Component, OnDestroy } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  resetEmail = new FormControl('', Validators.compose([Validators.required, Validators.email]));
  validationMessages = validationMessages;

  constructor(public authService: AuthService, private notify: NotificationService, private router: Router) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  sendForgotPasswordLink(email: string): void {
    this.authService
      .sendForgotPasswordLink(email)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(() => {
        this.notify.success('Please check your inbox for the reset link');
        this.resetEmail.reset();
        this.router.navigate(['/login']);
      });
  }
}
