import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  templateUrl: './prevent-unsaved.component.html',
  styleUrls: ['./prevent-unsaved.component.css']
})
export class PreventUnsavedComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<PreventUnsavedComponent>
  ) {}

  cancel(): void {
    return this.dialogRef.close();
  }

  discard(): void {
    return this.dialog.closeAll();
  }
}
