import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  templateUrl: './user-profile-edit.component.html',
  styleUrls: ['./user-profile-edit.component.css']
})
export class UserProfileEditComponent implements OnInit {
  userForm!: FormGroup;
  user!: User;
  validationMessages = {
    firstName: [
      { type: 'required', message: 'First Name is required' },
      { type: 'maxlength', message: 'First Name cannot be more than 25 characters' }
    ],
    lastName: [
      { type: 'required', message: 'Last Name is required' },
      { type: 'maxlength', message: 'Last Name cannot be more than 25 characters' }
    ],
    phoneNumber: [{ type: 'required', message: 'Phone Number is required' }],
    gender: [{ type: 'required', message: 'Gender is required' }]
  };
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<UserProfileEditComponent>,
    private dialog: MatDialog,
    public notify: NotificationService,
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.user = this.data.userForUpdate;
    this.populateForm(this.data.userForUpdate);
  }

  populateForm(member: User): void {
    // TODO add phone number validation
    this.userForm = this.fb.group({
      id: new FormControl(member.id),
      firstName: new FormControl(member.firstName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl(member.lastName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      phoneNumber: new FormControl(member.phoneNumber, Validators.compose([Validators.required])),
      gender: new FormControl(member.gender, Validators.required),
      email: new FormControl(member.email),
      created: new FormControl(member.created),
      role: new FormControl(member.role),
      photoUrl: new FormControl(member.photoUrl)
    });
  }

  closeDialog(): void {
    if (this.userForm.dirty) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  onSubmit(): void {
    this.updateUser(this.userForm.value);
  }

  updateUser(user: User): void {
    this.userService.updateUser(this.authService.loggedInUser.id, user).subscribe(
      () => {
        this.mapUpdatedUser(user);
        this.dialogRef.close(this.user);
        this.notify.success('Updated Successful');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  mapUpdatedUser(user: User): void {
    this.user.firstName = user.firstName;
    this.user.lastName = user.lastName;
    this.user.phoneNumber = user.phoneNumber;
    this.user.gender = user.gender;
  }
}
