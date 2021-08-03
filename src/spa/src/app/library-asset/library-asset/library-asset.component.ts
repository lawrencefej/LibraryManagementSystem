import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
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

  @ViewChild('categoryInput') categoryInput: ElementRef<HTMLInputElement>;
  @ViewChild('authorInput') authorInput: ElementRef<HTMLInputElement>;
  assetForm!: FormGroup;
  assetType = LibraryAssetType;
  categories: CategoryDto[] = [];
  authors: AuthorDto[] = [];
  filteredCategories$?: Observable<CategoryDto[]> = of([]);
  filteredAuthors$?: Observable<AuthorDto[]> = of([]);
  // selectedAuthors: AuthorDto[] = [
  //   { id: 1, fullName: 'Ashley Rennebeck' },
  //   { id: 2, fullName: 'Carlina Schuster' }
  // ];
  // selectedCategories: CategoryDto[] = [
  //   { id: 1, name: 'Computer science and Information Technology' },
  //   { id: 2, name: 'Philosophy and psychology' }
  // ];
  selectedAuthors: AuthorDto[] = [];
  selectedCategories: CategoryDto[] = [];
  serverValidationErrors: string[] = [];
  validationMessages = libraryAssetValidationMessages;
  categoryForm = new FormControl('', Validators.required);
  authorForm = new FormControl('', Validators.required);

  constructor(
    private readonly assetService: LibraryAssetService,
    private readonly categoryService: CategoryService,
    private readonly dialog: MatDialog,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService,
    private readonly router: Router
  ) {
    this.getCategoryList();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.createForm();
    this.watchAssetTypeChanges();
    // this.watchCategoryChanges();
    this.watchCategoryFormChanges();
    this.watchAuthorFormChanges();
    // this.getCategoryList();
    this.getAuthors();
    this.filteredCategories$.subscribe(data => {
      console.log(data);
    });
  }

  // watchCategoryChanges(): void {
  //   this.categoryForm.valueChanges.pipe(takeUntil(this.unsubscribe)).subscribe(() => {
  //     // const category = this.categoryForm.value;

  //     // if (category) {
  //     //   this.selectedCategories.push(category);
  //     // }
  //     console.log(this.categories);
  //   });
  // }

  addAsset(): void {
    console.log(this.assetForm.value);
  }

  displayCategoryName(category: CategoryDto): string {
    return category.name;
  }

  displayAuthorName(author: AuthorDto): string {
    return author.fullName;
  }

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

  // get assetCategories(): FormArray {
  //   return this.assetForm.get('assetCategories') as FormArray;
  // }

  // addFormCategory(): void {
  //   console.log(this.assetForm.get('assetCategories')?.value);
  //   console.log(this.assetForm.value);

  //   this.assetCategories.push(this.addAssetCategory());
  // }

  // removeFormCategory(index: number): void {
  //   this.assetCategories.removeAt(index);
  // }

  // private getCategories(): void {
  //   this.categoryService.categories$.pipe(takeUntil(this.unsubscribe));
  // }

  private addAssetCategory(): FormGroup {
    return this.fb.group({
      category: ['', Validators.compose([Validators.required])],
      categoryId: ['', Validators.compose([Validators.required])]
    });
  }

  private createForm(): void {
    this.assetForm = this.fb.group({
      assetAuthors: new FormControl('', Validators.compose([Validators.required])),
      // assetCategories: new FormControl('', Validators.required),
      // assetCategories: this.fb.array([new FormControl('', Validators.required)]),
      assetCategories: this.fb.array([this.addAssetCategory()]),
      assetType: new FormControl('', Validators.required),
      description: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(200)])),
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
    this.categoryService.categories$.pipe(takeUntil(this.unsubscribe)).subscribe(categories => {
      this.categories = categories;
      console.log(categories);
    });
  }

  // private filterCategories(value: any): CategoryDto[] {
  //   let filterValue: string;

  //   value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

  //   const initial = this.categories.filter(category => category.name.toLowerCase().includes(filterValue));

  //   // Filter selected categories
  //   return initial.filter(
  //     category => !this.selectedCategories.find(selectedCategory => category.id === selectedCategory.id)
  //   );
  // }

  private filterCategories(value: any): CategoryDto[] {
    let filterValue: string;

    value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

    const initial = this.categories.filter(category => category.name.toLowerCase().includes(filterValue));

    // Filter selected categories
    return initial.filter(
      category => !this.selectedCategories.find(selectedCategory => category.id === selectedCategory.id)
    );
    // return this.categories.filter(category => category.name.toLowerCase().includes(filterValue));
  }

  private watchAuthorFormChanges(): void {
    this.filteredAuthors$ = this.authorForm.valueChanges.pipe(
      startWith(''),
      map(s => this.filterAuthors(s))
    );
  }

  private filterAuthors(value: any): AuthorDto[] {
    console.log(value);

    let filterValue: string;

    // value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

    if (value.id > 0) {
      filterValue = value.fullName.toLowerCase();
    } else {
      // if (value !== undefined) {
      filterValue = value.toLowerCase();
      // }
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
