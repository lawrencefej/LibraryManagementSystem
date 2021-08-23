import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MustMatch } from 'src/app/shared/validators/password-match.validator';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { UserForDetailedDto, UserPasswordResetRequest } from 'src/dto/models';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'lms-user-password-change',
  templateUrl: './user-password-change.component.html',
  styleUrls: ['./user-password-change.component.css']
})
export class UserPasswordChangeComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input()
  user!: UserForDetailedDto;

  changeForm!: FormGroup;
  validationMessages = validationMessages;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly notify: NotificationService,
    private readonly userService: UserService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.createForm();
  }

  onSubmit(resetRequest: UserPasswordResetRequest): void {
    this.userService
      .resetUserPassword(resetRequest)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(() => {
        this.notify.success('Password changed successfully');
      });
  }

  private createForm(): void {
    this.changeForm = this.formBuilder.group(
      {
        userId: new FormControl(this.user.id),
        currentPassword: new FormControl('', Validators.compose([Validators.required])),
        newPassword: new FormControl('', Validators.compose([Validators.required])),
        confirmPassword: new FormControl('', Validators.compose([Validators.required]))
      },
      {
        validators: MustMatch('newPassword', 'confirmPassword')
      }
    );
  }
}
