import { Component, OnInit, Inject } from '@angular/core';

import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AdminService } from 'src/app/_services/admin.service';
import { User } from 'src/app/_models/user';
import { NotificationService } from 'src/app/_services/notification.service';
import { Role } from 'src/app/_models/role';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  userForm: FormGroup;
  user: User;
  showRevert = false;

  validationMessages = {
    firstName: [
      { type: 'required', message: 'First Name is required' },
      {
        type: 'maxlength',
        message: 'First Name cannot be more than 25 characters'
      }
    ],
    lastName: [
      { type: 'required', message: 'Last Name is required' },
      {
        type: 'maxlength',
        message: 'Last Name cannot be more than 25 characters'
      }
    ],
    email: [
      { type: 'required', message: 'Email is required' },
      { type: 'email', message: 'Please enter a valid Email' }
    ],
    role: [
      { type: 'required', message: 'Role is required' },
      { type: 'required', message: 'Please select a valid role' }
    ]
  };

  roles: Role[] = [
    { id: 1, name: 'Librarian' },
    { id: 2, name: 'Admin' }
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AdminComponent>,
    private dialog: MatDialog,
    public notify: NotificationService,
    private adminService: AdminService,
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.createUserForm();
    this.isUpdate();
  }

  createUserForm(): void {
    this.userForm = this.fb.group({
      id: new FormControl(0),
      firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      email: new FormControl('', Validators.compose([Validators.required, Validators.email])),
      role: new FormControl('', Validators.compose([Validators.required]))
    });
  }

  populateForm(user: User): void {
    this.userForm = this.fb.group({
      id: new FormControl(user.id),
      firstName: new FormControl(user.firstName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl(user.lastName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      email: new FormControl(user.email, Validators.compose([Validators.required, Validators.email])),
      role: new FormControl(user.role, Validators.compose([Validators.required]))
    });
    this.userForm.controls.firstName.disable();
    this.userForm.controls.lastName.disable();
    this.userForm.controls.email.disable();
  }

  isUpdate(): void {
    if (this.data) {
      this.populateForm(this.data);
      this.user = this.data;
      this.showRevert = true;
    } else {
      this.createUserForm();
    }
  }

  revert(): void {
    this.populateForm(this.user);
  }

  closeDialog() {
    if (this.userForm.dirty) {
      this.notify.discardDialog('Are you sure you want to delete this user?');
    } else {
      this.dialog.closeAll();
    }
  }

  onSubmit(): void {
    if (this.userForm.controls.id.value) {
      this.userForm.controls.firstName.enable();
      this.userForm.controls.lastName.enable();
      this.userForm.controls.email.enable();
      this.updateUserRole(this.userForm.value);
    } else {
      this.addUser(this.userForm.value);
    }
  }

  addUser(user: User): void {
    this.adminService.addUser(user).subscribe(
      (createdMember: User) => {
        user = createdMember;
        this.dialogRef.close(createdMember);
        this.notify.success('User Added Successfully');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  updateUserRole(user: User): void {
    this.adminService.updateUser(user).subscribe(
      () => {
        this.dialogRef.close(user);
        this.notify.success('Updated Role Successful');
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
