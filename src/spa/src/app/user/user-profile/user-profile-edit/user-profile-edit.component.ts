import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { messages } from 'src/app/shared/message.constants';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { MemberGenderDto, UserForDetailedDto, UserForUpdateDto } from 'src/dto/models';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'lms-user-profile-edit',
  templateUrl: './user-profile-edit.component.html',
  styleUrls: ['./user-profile-edit.component.css']
})
export class UserProfileEditComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input() user!: UserForDetailedDto;
  @Input() isFormDirty = false;

  @Output() userChange = new EventEmitter<UserForDetailedDto>();
  @Output() isFormDirtyChange = new EventEmitter<boolean>();
  @Output() closeTab = new EventEmitter<void>();

  genders = MemberGenderDto;
  serverValidationErrors: string[] = [];
  userForm!: FormGroup;
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
    this.populateForm(this.user);
    this.userForm.valueChanges
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(() => this.isFormDirtyChange.emit(this.userForm.dirty));
  }

  cancelEdit(): void {
    this.closeTab.emit();
  }

  updateProfile(model: UserForUpdateDto): void {
    this.userService
      .updatedUserProfile(model)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        updatedUser => {
          this.userChange.emit(updatedUser);
          this.userForm.reset();
          this.notify.success('Profile updated successfully');
          this.closeTab.emit();
        },
        error => {
          this.serverValidationErrors = error;
        }
      );
  }

  revert(): void {
    this.notify
      .confirm(messages.discard.main, messages.discard.submsg)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(response => {
        if (response) {
          this.populateForm(this.user);
          this.isFormDirtyChange.emit(false);
        }
      });
  }

  private populateForm(user: UserForDetailedDto): void {
    this.userForm = this.formBuilder.group({
      id: new FormControl(user.id),
      firstName: new FormControl(user.firstName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl(user.lastName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      phoneNumber: new FormControl(
        user.phoneNumber,
        Validators.compose([Validators.required, Validators.maxLength(15), Validators.pattern('^[0-9]{10}$')])
      ),
      gender: new FormControl(user.gender, Validators.required)
    });
  }
}
