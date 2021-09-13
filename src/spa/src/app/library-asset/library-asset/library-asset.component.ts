import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { messages } from 'src/app/shared/message.constants';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { libraryAssetValidationMessages } from 'src/app/shared/validators/validator.constants';
import {
  AuthorDto,
  CategoryDto,
  LibraryAssetAuthorDto,
  LibraryAssetCategoryDto,
  LibraryAssetForCreationDto,
  LibraryAssetType
} from 'src/dto/models';
import { CategoryService } from '../services/category.service';
import { LibraryAssetService } from '../services/library-asset.service';

@Component({
  selector: 'lms-library-asset',
  templateUrl: './library-asset.component.html',
  styleUrls: ['./library-asset.component.css']
})
export class LibraryAssetComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @ViewChild('authorInput') authorInput!: ElementRef<HTMLInputElement>;
  @ViewChild('categoryInput') categoryInput!: ElementRef<HTMLInputElement>;
  assetForm!: FormGroup;
  assetType = LibraryAssetType;
  authorForm = new FormControl('', Validators.required);
  authors: AuthorDto[] = [];
  categories: CategoryDto[] = [];
  categoryForm = new FormControl('', Validators.required);
  filteredAuthors$?: Observable<AuthorDto[]> = of([]);
  filteredCategories$?: Observable<CategoryDto[]> = of([]);
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
    this.watchCategoryFormChanges();
    this.watchAuthorFormChanges();
    this.getCategoryList();
    this.getAuthors();
  }

  addAsset(asset: LibraryAssetForCreationDto): void {
    asset.assetAuthors = this.mapSelectedAuthor();
    asset.assetCategories = this.mapSelectedCategories();

    this.assetService
      .addAsset(asset)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        returnedAsset => {
          this.dialog.closeAll();
          this.router.navigateByUrl(`/catalog/detail/${returnedAsset.id}`);
          this.notify.success('Card added successfully');
        },
        error => (this.serverValidationErrors = error)
      );
  }

  displayCategoryName(category: CategoryDto): string {
    return category.name;
  }

  displayAuthorName(author: AuthorDto): string {
    return author.fullName;
  }

  cancel(): void {
    if (this.assetForm.dirty || this.selectedAuthors.length > 0 || this.selectedCategories.length > 0) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  reset(): void {
    this.notify
      .confirm(messages.discard.main, messages.discard.submsg)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(respose => {
        if (respose) {
          this.assetForm.reset();
          this.selectedAuthors = [];
          this.selectedCategories = [];
        }
      });
  }

  removeAuthor(author: AuthorDto): void {
    const index = this.selectedAuthors.indexOf(author);
    this.selectedAuthors.splice(index, 1);
  }

  addCategory(category: CategoryDto): void {
    if (category.id === undefined || category.id > 0) {
      this.selectedCategories.push(category);
      this.categoryInput.nativeElement.value = '';
      this.categoryForm.setValue('');
    }
  }

  dropCategory(event: CdkDragDrop<CategoryDto[]>): void {
    moveItemInArray(this.selectedCategories, event.previousIndex, event.currentIndex);
  }

  dropAuthor(event: CdkDragDrop<AuthorDto[]>): void {
    moveItemInArray(this.selectedAuthors, event.previousIndex, event.currentIndex);
  }

  addAuthor(author: AuthorDto): void {
    if (author.id > 0) {
      this.selectedAuthors.push(author);
      this.authorInput.nativeElement.value = '';
      this.authorForm.setValue('');
    }
  }

  removeCategory(category: CategoryDto): void {
    const index = this.selectedCategories.indexOf(category);
    this.selectedCategories.splice(index, 1);
  }

  disableSubmit(): boolean {
    return (
      this.assetForm.invalid ||
      this.assetForm.pristine ||
      this.selectedAuthors.length <= 0 ||
      this.selectedCategories.length <= 0
    );
  }

  private mapSelectedCategories(): LibraryAssetCategoryDto[] {
    return this.selectedCategories.map(a => ({ categoryId: a.id! }));
  }

  private mapSelectedAuthor(): LibraryAssetAuthorDto[] {
    return this.selectedAuthors.map((a, index) => ({ authorId: a.id, order: index }));
  }

  private watchAssetTypeChanges(): void {
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

  private createForm(): void {
    this.assetForm = this.fb.group({
      assetType: new FormControl('', Validators.required),
      description: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(250)])),
      isbn: new FormControl({ value: '', disabled: true }, Validators.required),
      numberOfCopies: new FormControl('', Validators.required),
      title: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      year: new FormControl('', Validators.compose([Validators.required, Validators.pattern('^[0-9]{4}$')]))
    });
  }

  private watchCategoryFormChanges(): void {
    this.filteredCategories$ = this.categoryForm.valueChanges.pipe(
      startWith(''),
      map(s => this.filterCategories(s))
    );
  }

  private getCategoryList(): void {
    this.categoryService.categories$
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(categories => (this.categories = categories));
  }

  private filterCategories(value: any): CategoryDto[] {
    let filterValue: string;

    value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

    const initial = this.categories.filter(category => category.name.toLowerCase().includes(filterValue));

    // Filter selected categories
    return initial.filter(
      category => !this.selectedCategories.find(selectedCategory => category.id === selectedCategory.id)
    );
  }

  private watchAuthorFormChanges(): void {
    this.filteredAuthors$ = this.authorForm.valueChanges.pipe(
      startWith(''),
      map(s => this.filterAuthors(s))
    );
  }

  private filterAuthors(value: any): AuthorDto[] {
    let filterValue: string;

    // value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

    if (value.id > 0) {
      filterValue = value.fullName.toLowerCase();
    } else {
      filterValue = value.toLowerCase();
    }

    const initial = this.authors.filter(author => author.fullName.toLowerCase().includes(filterValue));

    // Filter selected authors
    return initial.filter(author => !this.selectedAuthors.find(selectedAuthor => author.id === selectedAuthor.id));
  }

  private getAuthors(): void {
    this.authorForm.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(value => this.assetService.getAuthors(value))
      )
      .subscribe(authors => (this.authors = authors));
  }
}
