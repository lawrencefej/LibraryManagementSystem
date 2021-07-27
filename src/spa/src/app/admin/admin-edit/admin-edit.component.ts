import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { AdminUserForListDto, UserRoleDto } from 'src/dto/models';
import { UpdateAdminRoleDto } from 'src/dto/models/update-admin-role-dto';
import { AdminService } from '../services/admin.service';

@Component({
  templateUrl: './admin-edit.component.html',
  styleUrls: ['./admin-edit.component.css']
})
export class AdminEditComponent implements OnInit {
  private readonly unsubscribe = new Subject<void>();

  validationMessages = validationMessages;
  userForm!: FormGroup;
  userRoles: UserRoleDto[] = [
    { id: 1, name: 'Librarian' },
    { id: 2, name: 'Admin' }
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: AdminUserForListDto,
    private readonly fb: FormBuilder,
    public readonly dialogRef: MatDialogRef<AdminEditComponent>,
    private readonly dialog: MatDialog,
    public readonly notify: NotificationService,
    private readonly adminService: AdminService
  ) {}

  ngOnInit(): void {
    this.populateForm(this.data);
  }

  populateForm(user: AdminUserForListDto): void {
    this.userForm = this.fb.group({
      id: new FormControl(user.id),
      firstName: new FormControl(
        { value: user.firstName, disabled: true },
        Validators.compose([Validators.required, Validators.maxLength(25)])
      ),
      lastName: new FormControl(
        { value: user.lastName, disabled: true },
        Validators.compose([Validators.required, Validators.maxLength(25)])
      ),
      email: new FormControl(
        { value: user.email, disabled: true },
        Validators.compose([Validators.required, Validators.email])
      ),
      role: new FormControl(user.role.name, Validators.compose([Validators.required]))
    });
  }

  editAdmin(user: UpdateAdminRoleDto): void {
    this.adminService
      .updateUser(user)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(() => {
        this.userForm.controls.email.enable();
        this.dialogRef.close(this.userForm.value);
        this.notify.success('Updated Role Successful');
      });
  }

  revert(): void {
    this.notify
      .confirm('Are you sure you want to discard these changes?')
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(response => {
        if (response) {
          this.populateForm(this.data);
        }
      });
  }

  closeDialog(): void {
    if (this.userForm.dirty) {
      this.notify.discardDialog('Are you sure you want to delete this user?');
    } else {
      this.dialog.closeAll();
    }
  }
}
