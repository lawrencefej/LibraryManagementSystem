import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

import { ConfirmDialogComponent } from '../shared/confirm-dialog/confirm-dialog.component';
import { Injectable } from '@angular/core';
import { PreventUnsavedComponent } from '../shared/prevent-unsaved/prevent-unsaved.component';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(public snackBar: MatSnackBar, private dialog: MatDialog) {}
  action = 'Dismiss';

  config: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'right',
    verticalPosition: 'bottom',
    politeness: 'assertive'
  };

  success(message: string): void {
    this.config.panelClass = ['notification', 'success'];
    this.snackBar.open(message, this.action, this.config);
  }

  error(message: string): void {
    this.action.fontcolor('black');
    this.config.panelClass = ['notification', 'error'];
    this.snackBar.open(message, this.action, this.config);
  }

  warn(message: string): void {
    this.config.panelClass = ['notification', 'warn'];
    this.snackBar.open(message, this.action, this.config);
  }

  message(message: string): void {
    this.config.panelClass = ['notification', 'message'];
    this.snackBar.open(message, this.action, this.config);
  }

  private getDialogConfig(msg: string): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '340px';
    dialogConfig.data = {
      message: msg
    };

    return dialogConfig;
  }

  confirm(msg: string): MatDialogRef<ConfirmDialogComponent, boolean> {
    const dialogConfig = this.getDialogConfig(msg);

    return this.dialog.open(ConfirmDialogComponent, dialogConfig);
  }

  discardDialog(msg: string): MatDialogRef<PreventUnsavedComponent, void> {
    const dialogConfig = this.getDialogConfig(msg);

    return this.dialog.open(PreventUnsavedComponent, dialogConfig);
  }
}
