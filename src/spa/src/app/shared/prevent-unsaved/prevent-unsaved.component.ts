import { Component, OnInit, Inject } from '@angular/core';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';

@Component({
  selector: 'app-prevent-unsaved',
  templateUrl: './prevent-unsaved.component.html',
  styleUrls: ['./prevent-unsaved.component.css'],
})
export class PreventUnsavedComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<PreventUnsavedComponent>
  ) {}

  ngOnInit() {}

  cancel() {
    this.dialogRef.close();
  }

  discard() {
    this.dialog.closeAll();
  }
}
