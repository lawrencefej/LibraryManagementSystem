import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { AuthService } from 'src/app/_services/authentication.service';
import { UserForLoginDto } from 'src/dto/models';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  loginForm!: FormGroup;
  returnUrl!: string;
  validationMessages = validationMessages;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly fb: FormBuilder,
    public readonly notification: NotificationService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/home';
    this.createLoginForm();
  }

  onSubmit(model: UserForLoginDto): void {
    if (this.loginForm.valid) {
      this.authService
        .login(model)
        .pipe(takeUntil(this.unsubscribe))
        .subscribe(() => {
          this.router.navigateByUrl(this.returnUrl);
        });
    }
  }

  createLoginForm(): void {
    this.loginForm = this.fb.group({
      email: new FormControl(
        '',
        Validators.compose([Validators.required, Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')])
      ),
      password: new FormControl('', Validators.compose([Validators.required, Validators.minLength(4)]))
    });
  }
}
