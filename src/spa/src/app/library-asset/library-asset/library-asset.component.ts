import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { libraryAssetValidationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { AuthorDto, CategoryDto, LibraryAssetType } from 'src/dto/models';
import { CategoryService } from '../services/category.service';
import { LibraryAssetService } from '../services/library-asset.service';

@Component({
  selector: 'lms-library-asset',
  templateUrl: './library-asset.component.html',
  styleUrls: ['./library-asset.component.css']
})
export class LibraryAssetComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  assetForm!: FormGroup;
  assetType = LibraryAssetType;
  selectedAuthors: AuthorDto[] = [];
  selectedCategories: CategoryDto[] = [];
  serverValidationErrors: string[] = [];
  validationMessages = libraryAssetValidationMessages;

  constructor(
    private readonly assetService: LibraryAssetService,
    private readonly categoryService: CategoryService,
    private readonly dialog: MatDialog,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService,
    private readonly router: Router
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.createForm();
    this.watchAssetTypeChanges();
  }

  addAsset(): void {}

  // private getAuthors(): void {
  //   this.assetForm.get('')?.valueChanges.pipe(
  //     takeUntil(this.unsubscribe),
  //     debounceTime(500),
  //     distinctUntilChanged(),
  //     switchMap(() => this.assetService.getAuthors(this.assetForm.get('')?.value))
  //   );
  // }
  cancel(): void {
    if (this.assetForm.dirty) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  watchAssetTypeChanges(): void {
    this.assetForm
      .get('assetType')
      ?.valueChanges.pipe(takeUntil(this.unsubscribe))
      .subscribe(value => {
        if (value === this.assetType.Book) {
          this.assetForm.controls.isbn.enable();
        } else {
          this.assetForm.controls.isbn.disable();
        }
      });
  }

  removeCategory(category: CategoryDto): void {
    const index = this.selectedCategories.indexOf(category);

    if (index >= 0) {
      this.selectedCategories.splice(index, 1);
    }
  }

  private getCategories(): void {
    this.categoryService.categories$.pipe(takeUntil(this.unsubscribe));
  }

  private createForm(): void {
    this.assetForm = this.fb.group({
      assetAuthors: new FormControl('', Validators.compose([Validators.required])),
      assetCategories: new FormControl('', Validators.required),
      assetType: new FormControl('', Validators.required),
      description: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(200)])),
      isbn: new FormControl({ value: '', disabled: true }, Validators.required),
      numberOfCopies: new FormControl('', Validators.required),
      title: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      year: new FormControl('', Validators.compose([Validators.required, Validators.pattern('^[0-9]{4}$')]))
    });
  }
}
