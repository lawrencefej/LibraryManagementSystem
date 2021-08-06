import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { cloneDeep } from 'lodash-es';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { messages } from 'src/app/shared/message.constants';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { AuthorDto } from 'src/dto/models';
import { AuthorService } from '../services/author.service';

export interface AuthorForCreation {
  description?: string;
  firstName: string;
  lastName: string;
  fullName: string;
  id: number;
}

@Component({
  selector: 'lms-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  author?: AuthorDto;
  authorForm: FormGroup;
  isEditAuthor = false;
  serverValidationErrors: string[] = [];
  validationMessages = validationMessages;

  constructor(
    private readonly dialog: MatDialog,
    private readonly fb: FormBuilder,
    private readonly authorService: AuthorService,
    private readonly router: Router,
    private readonly dialogRef: MatDialogRef<AuthorComponent>,
    private readonly notify: NotificationService,
    @Inject(MAT_DIALOG_DATA) private data?: AuthorDto
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.isEdit(this.data);
  }

  isEdit(author: AuthorDto): void {
    if (author) {
      this.populateForm(author);
      this.author = cloneDeep(author);
      this.isEditAuthor = true;
    } else {
      this.createForm();
    }
  }

  addAuthor(author: AuthorForCreation): void {
    author.fullName = `${author.firstName} ${author.lastName}`;

    this.authorService
      .addAuthor(author)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        returnAuthor => {
          this.dialog.closeAll();
          this.router.navigateByUrl(`/authors/detail/${returnAuthor.id}`);
          this.notify.success('author was added successfully');
        },
        error => (this.serverValidationErrors = error)
      );
  }

  updateAuthor(author: AuthorDto): void {
    this.authorService
      .updateAuthor(author)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        returnAuthor => {
          this.dialogRef.close(returnAuthor);
          this.notify.success('author was updated successfully');
        },
        error => (this.serverValidationErrors = error)
      );
  }

  cancel(): void {
    if (this.authorForm.dirty) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  revert(): void {
    this.populateForm(this.author);
  }

  reset(): void {
    this.notify
      .confirm(messages.discard.main, messages.discard.submsg)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(respose => {
        if (respose) {
          this.authorForm.reset();
        }
      });
  }

  private createForm(): void {
    this.authorForm = this.fb.group({
      firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      description: new FormControl('', Validators.compose([Validators.maxLength(250)]))
    });
  }

  private populateForm(author: AuthorDto): void {
    this.authorForm = this.fb.group({
      id: new FormControl(author.id),
      // TODO Validate full name or split into multiple properties
      fullName: new FormControl(author.fullName, Validators.compose([Validators.required, Validators.maxLength(50)])),
      description: new FormControl(author.description || '', Validators.compose([Validators.maxLength(250)]))
    });
  }
}
