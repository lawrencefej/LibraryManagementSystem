import { Component, Inject, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { AddAdminDto, AdminUserForListDto, UserRoleDto } from 'src/dto/models';
import { AdminService } from '../services/admin.service';

@Component({
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  userForm = new FormGroup({
    firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
    lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
    email: new FormControl('', Validators.compose([Validators.required, Validators.email])),
    role: new FormControl('', Validators.compose([Validators.required]))
  });
  userRoles: UserRoleDto[] = [
    { id: 1, name: 'Librarian' },
    { id: 2, name: 'Admin' }
  ];
  validationMessages = validationMessages;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: AdminUserForListDto,
    public readonly dialogRef: MatDialogRef<AdminComponent>,
    private readonly dialog: MatDialog,
    public readonly notify: NotificationService,
    private readonly adminService: AdminService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  closeDialog(): void {
    if (this.userForm.dirty) {
      this.notify.discardDialog('Are you sure you want to delete this user?');
    } else {
      this.dialog.closeAll();
    }
  }

  addUser(user: AddAdminDto): void {
    this.adminService.addUser(user).subscribe(
      createdMember => {
        this.dialogRef.close(createdMember);
        this.notify.success('User Added Successfully');
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
