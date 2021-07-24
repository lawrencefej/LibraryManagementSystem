import { Component, OnInit, Inject } from '@angular/core';

import { Author } from 'src/app/_models/author';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NotificationService } from 'src/app/_services/notification.service';
import { AuthorService } from 'src/app/_services/author.service';
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

@Component({
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit {
  authorForm: FormGroup;
  author: Author;
  showReset = false;

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
    ]
  };

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Author,
    private router: Router,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AuthorComponent>,
    private dialog: MatDialog,
    public notify: NotificationService,
    private authorService: AuthorService
  ) {}

  ngOnInit(): void {
    this.createAuthorForm();
    this.isUpdate();
  }

  createAuthorForm(): void {
    this.authorForm = this.fb.group({
      id: new FormControl(0),
      firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)]))
    });
  }

  isUpdate(): void {
    if (this.data) {
      this.populateForm(this.data);
      this.author = this.data;
      this.showReset = true;
    } else {
      this.createAuthorForm();
    }
  }

  populateForm(data: Author): void {
    this.authorForm = this.fb.group({
      id: new FormControl(data.id),
      firstName: new FormControl(data.firstName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl(data.lastName, Validators.compose([Validators.required, Validators.maxLength(25)]))
    });
  }

  revert(): void {
    this.populateForm(this.author);
  }

  closeDialog(): void {
    if (this.authorForm.dirty) {
      this.notify.discardDialog('Are you sure you want to delete this author');
    } else {
      this.dialog.closeAll();
    }
  }

  onSubmit(): void {
    if (this.authorForm.controls.id.value) {
      this.updateAuthor(this.authorForm.value);
    } else {
      this.addAuthor(this.authorForm.value);
    }
  }

  addAuthor(author: Author): void {
    this.authorService.addAuthor(author).subscribe(
      (createdAuthor: Author) => {
        this.dialogRef.close();
        this.router.navigate(['/authors', createdAuthor.id]);
        this.notify.success('Author Added Successfully');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  updateAuthor(author: Author): void {
    this.authorService.updateAuthors(author).subscribe(
      () => {
        this.dialogRef.close(author);
        this.router.navigate(['/authors', author.id]);
        this.notify.success('Updated author successfully');
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
