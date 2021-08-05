import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { cloneDeep, isEqual } from 'lodash-es';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { messages } from 'src/app/shared/message.constants';
import { libraryAssetValidationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import {
  AuthorDto,
  CategoryDto,
  LibraryAssetAuthorDto,
  LibraryAssetCategoryDto,
  LibraryAssetForDetailedDto,
  LibraryAssetForUpdateDto,
  LibraryAssetType
} from 'src/dto/models';
import { CategoryService } from '../../services/category.service';
import { LibraryAssetService } from '../../services/library-asset.service';

@Component({
  selector: 'lms-library-asset-edit',
  templateUrl: './library-asset-edit.component.html',
  styleUrls: ['./library-asset-edit.component.css']
})
export class LibraryAssetEditComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input() asset!: LibraryAssetForDetailedDto;
  @Output() assetChange = new EventEmitter<LibraryAssetForDetailedDto>();
  @Output() closeTab = new EventEmitter<void>();
  @Output() isFormDirty = new EventEmitter<boolean>();
  @ViewChild('authorInput') authorInput: ElementRef<HTMLInputElement>;
  @ViewChild('categoryInput') categoryInput: ElementRef<HTMLInputElement>;

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
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.populateForm(this.asset);
    this.watchAssetTypeChanges();
    this.watchCategoryFormChanges();
    this.watchAuthorFormChanges();
    this.getCategoryList();
    this.getAuthors();
    this.assetForm.valueChanges.pipe(takeUntil(this.unsubscribe)).subscribe(() => {
      this.isFormDirty.emit(this.assetForm.dirty);
    });
  }

  editAsset(asset: LibraryAssetForUpdateDto): void {
    asset.assetAuthors = this.mapSelectedAuthor();
    asset.assetCategories = this.mapSelectedCategories();

    this.assetService
      .updateAsset(asset)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        returnedAsset => {
          this.isFormDirty.emit(false);
          this.closeTab.emit();
          this.notify.success('Card updated successfully');
          this.assetChange.emit(returnedAsset);
        },
        error => (this.serverValidationErrors = error)
      );
  }

  addAuthor(author: AuthorDto): void {
    if (author.id > 0) {
      this.selectedAuthors.push(author);
      this.authorInput.nativeElement.value = '';
      this.authorForm.setValue('');
    }
  }

  removeAuthor(author: AuthorDto): void {
    const index = this.selectedAuthors.indexOf(author);
    this.selectedAuthors.splice(index, 1);
  }

  addCategory(category: CategoryDto): void {
    if (category.id > 0) {
      this.selectedCategories.push(category);
      this.categoryInput.nativeElement.value = '';
      this.categoryForm.setValue('');
    }
  }

  removeCategory(category: CategoryDto): void {
    const index = this.selectedCategories.indexOf(category);
    this.selectedCategories.splice(index, 1);
  }

  cancelEdit(): void {
    this.closeTab.emit();
  }

  displayCategoryName(category: CategoryDto): string {
    return category.name;
  }

  displayAuthorName(author: AuthorDto): string {
    return author.fullName;
  }

  dropCategory(event: CdkDragDrop<CategoryDto[]>): void {
    moveItemInArray(this.selectedCategories, event.previousIndex, event.currentIndex);
  }

  dropAuthor(event: CdkDragDrop<AuthorDto[]>): void {
    moveItemInArray(this.selectedAuthors, event.previousIndex, event.currentIndex);
  }

  reset(): void {
    this.notify
      .confirm(messages.discard.main, messages.discard.submsg)
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(respose => {
        if (respose) {
          this.populateForm(this.asset);

          this.selectedAuthors = cloneDeep(this.asset.authors);
          this.selectedCategories = cloneDeep(this.asset.categories);
          this.isFormDirty.emit(false);
        }
      });
  }

  disableReset(): boolean {
    return this.assetForm.pristine && this.areAuthorsCategoriesUnChanged();
  }

  disableSubmit(): boolean {
    return this.assetForm.invalid || (this.assetForm.pristine && this.areAuthorsCategoriesUnChanged());
  }

  private areAuthorsCategoriesUnChanged(): boolean {
    return isEqual(this.asset.authors, this.selectedAuthors) && isEqual(this.asset.categories, this.selectedCategories);
  }

  private areArraysEqual(first: any[], second: any[]): boolean {
    return JSON.stringify(first) === JSON.stringify(second);
  }

  private mapSelectedCategories(): LibraryAssetCategoryDto[] {
    return this.selectedCategories.map(a => ({ categoryId: a.id }));
  }

  private mapSelectedAuthor(): LibraryAssetAuthorDto[] {
    return this.selectedAuthors.map((a, index) => ({ authorId: a.id, order: index }));
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

  private getCategoryList(): void {
    this.categoryService.categories$
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(categories => (this.categories = categories));
  }

  private watchAuthorFormChanges(): void {
    this.filteredAuthors$ = this.authorForm.valueChanges.pipe(
      startWith(''),
      map(s => this.filterAuthors(s))
    );
  }

  private filterAuthors(value: any): AuthorDto[] {
    let filterValue: string;

    if (value.id > 0) {
      filterValue = value.fullName.toLowerCase();
    } else {
      filterValue = value.toLowerCase();
    }

    const initial = this.authors.filter(author => author.fullName.toLowerCase().includes(filterValue));

    // Filter selected authors
    return initial.filter(author => !this.selectedAuthors.find(selectedAuthor => author.id === selectedAuthor.id));
  }

  private watchCategoryFormChanges(): void {
    this.filteredCategories$ = this.categoryForm.valueChanges.pipe(
      startWith(''),
      map(s => this.filterCategories(s))
    );
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

  private populateForm(asset: LibraryAssetForDetailedDto): void {
    this.assetForm = this.fb.group({
      id: new FormControl(asset.id),
      assetType: new FormControl(asset.assetType, Validators.required),
      copiesAvailable: new FormControl(asset.copiesAvailable, Validators.required),
      description: new FormControl(
        asset.description,
        Validators.compose([Validators.required, Validators.maxLength(250)])
      ),
      isbn: new FormControl({ value: asset.isbn, disabled: true }, Validators.required),
      numberOfCopies: new FormControl(asset.numberOfCopies, Validators.required),
      title: new FormControl(asset.title, Validators.compose([Validators.required, Validators.maxLength(50)])),
      year: new FormControl(
        asset.year,
        Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(4)])
      )
    });

    if (this.assetForm.get('assetType')?.value === this.assetType.Book) {
      this.assetForm.controls.isbn.enable();
    }

    this.selectedAuthors = cloneDeep(asset.authors);
    this.selectedCategories = cloneDeep(asset.categories);
  }
}
