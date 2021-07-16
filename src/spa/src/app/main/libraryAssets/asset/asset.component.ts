import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AssetType } from 'src/app/_models/assetType';
import { Author } from 'src/app/_models/author';
import { Category } from 'src/app/_models/category';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { AssetTypeService } from 'src/app/_services/asset-type.service';
import { AssetService } from 'src/app/_services/asset.service';
import { AuthorService } from 'src/app/_services/author.service';
import { CategoryService } from 'src/app/_services/category.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { Observable, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { LibraryAssetForUpdateDto } from 'src/dto/models/library-asset-for-update-dto';
import { LibraryAssetForCreationDto } from 'src/dto/models/library-asset-for-creation-dto';

@Component({
  selector: 'app-asset',
  templateUrl: './asset.component.html',
  styleUrls: ['./asset.component.css']
})
export class AssetComponent implements OnInit, OnDestroy {
  categories: Category[];
  assetTypes: AssetType[];
  authors: Author[];
  assetForm: FormGroup;
  asset: LibraryAsset;
  selectedAssetType: AssetType;
  selectedCategory: any;
  showRevert = false;
  categories$: Observable<Category[]>;
  assetTypes$: Observable<AssetType[]>;
  subs: Subscription[] = [];

  validationMessages = {
    title: [
      { type: 'required', message: 'Title is required' },
      { type: 'maxlength', message: 'Title cannot be more than 25 characters' }
    ],
    author: [
      { type: 'required', message: 'Author is required' },
      {
        type: 'maxlength',
        message: 'Author cannot be more than 25 characters'
      },
      { type: 'authorError', message: 'Please select a valid author' }
    ],
    year: [
      { type: 'required', message: 'Year is required' },
      { type: 'pattern', message: 'Please enter a valid year' }
    ],
    numberOfCopies: [{ type: 'required', message: 'Number of copies is required' }],
    description: [
      { type: 'required', message: 'Description is required' },
      {
        type: 'maxlength',
        message: 'description cannot be more than 500 characters'
      }
    ],
    categoryId: [{ type: 'required', message: 'Category is required' }],
    assetTypeId: [{ type: 'required', message: 'Type is required' }],
    isbn: [{ type: 'required', message: 'ISBN is required' }]
  };

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: LibraryAsset,
    private router: Router,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AssetComponent>,
    private dialog: MatDialog,
    private authorService: AuthorService,
    private assetService: AssetService,
    public notify: NotificationService,
    private assetTypeService: AssetTypeService,
    private categoryService: CategoryService
  ) {}

  ngOnInit() {
    this.categories$ = this.categoryService.getCategories();
    this.assetTypes$ = this.assetTypeService.getAssetTypes();
    this.subs.push(this.categories$.subscribe(_ => (this.categories = _)));
    this.subs.push(this.assetTypes$.subscribe(_ => (this.assetTypes = _)));
    this.isUpdate();
    this.getAuthor();
    this.authorChanges();
  }

  isUpdate() {
    if (!this.data) {
      this.createAssetForm();
    } else {
      if (this.data.id) {
        this.populateForm(this.data);
        this.showRevert = true;
      } else {
        this.createAssetForm();
        this.assetForm.controls.author.setValue(this.data.author);
      }
    }
  }

  revert() {
    this.populateForm(this.data);
  }

  createAssetForm() {
    this.assetForm = this.fb.group({
      id: new FormControl(0),
      title: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      author: new FormControl('', Validators.compose([Validators.required])),
      year: new FormControl('', Validators.compose([Validators.required, Validators.pattern('^[0-9]{4}$')])),
      numberOfCopies: new FormControl('', Validators.required),
      description: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(500)])),
      categoryId: new FormControl('', Validators.required),
      assetTypeId: new FormControl('', Validators.required),
      isbn: new FormControl({ value: '', disabled: true }, Validators.required)
    });
  }

  populateForm(asset: LibraryAsset) {
    this.assetForm = this.fb.group({
      id: new FormControl(asset.id),
      title: new FormControl(asset.title, Validators.compose([Validators.required, Validators.maxLength(25)])),
      author: new FormControl(asset.author, Validators.compose([Validators.required])),
      authorId: new FormControl(asset.author.id, Validators.compose([Validators.required])),
      year: new FormControl(
        asset.year,
        Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(4)])
      ),
      numberOfCopies: new FormControl(asset.numberOfCopies, Validators.required),
      copiesAvailable: new FormControl(asset.copiesAvailable, Validators.required),
      description: new FormControl(
        asset.description,
        Validators.compose([Validators.required, Validators.maxLength(500)])
      ),
      categoryId: new FormControl(asset.category.id, Validators.required),
      assetTypeId: new FormControl(asset.assetType.id, Validators.required),
      statusId: new FormControl(asset.assetType.id, Validators.required),
      isbn: new FormControl({ value: asset.isbn, disabled: true }, Validators.required)
    });
  }

  authorChanges() {
    this.assetForm.controls.author.valueChanges.subscribe((author: Author) => {
      if (author.id === undefined) {
        this.assetForm.controls.author.setErrors({ authorError: true });
      }
    });
  }

  getAuthor() {
    this.assetForm.controls.author.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(value => this.authorService.searchAuthors(value))
      )
      .subscribe(
        data => {
          this.authors = data;
        },
        error => {
          this.notify.error(error);
        }
      );
  }

  onItemChange(value: any) {
    if (value.id !== null) {
      if (Number(value) === this.getBookId()) {
        this.assetForm.controls.isbn.enable();
      } else {
        this.assetForm.controls.isbn.disable();
      }
    }
  }

  getBookId() {
    let assetTypeId: AssetType;
    this.assetTypes$.subscribe(types => {
      this.assetTypes = types;
    });
    assetTypeId = this.assetTypes.find(x => x.name === 'Book');
    return assetTypeId.id;
  }

  displayFn(author: Author) {
    return author ? author.fullName : undefined;
  }

  closeDialog() {
    if (this.assetForm.dirty) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  onSubmit() {
    if (this.assetForm.controls.id.value) {
      this.updateAsset(this.assetForm.value);
    } else {
      this.addAsset(this.assetForm.value);
    }
  }

  addAsset(asset: LibraryAssetForCreationDto) {
    this.assetService.addAsset(asset).subscribe(
      libraryAsset => {
        // (libraryAsset: LibraryAsset) => {
        // asset = libraryAsset;
        this.dialogRef.close();
        this.router.navigate(['/catalog', libraryAsset.id]);
        // this.router.navigate(['/catalog', asset.id]);
        this.notify.success('Item Added Successfully');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  // addAsset(asset: LibraryAsset) {
  //   this.assetService.addAsset(asset).subscribe(
  //     (libraryAsset: LibraryAsset) => {
  //       asset = libraryAsset;
  //       this.dialogRef.close();
  //       this.router.navigate(['/catalog', asset.id]);
  //       this.notify.success('Item Added Successfully');
  //     },
  //     (error) => {
  //       this.notify.error(error);
  //     }
  //   );
  // }

  updateAsset(asset: LibraryAssetForUpdateDto) {
    // asset.authorId = asset.author.id;
    this.assetService.updateAsset(asset).subscribe(
      () => {
        this.dialogRef.close();
        this.router.navigate(['/catalog', asset.id]);
        this.notify.success('Updated Successful');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  // updateAsset(asset: LibraryAsset) {
  //   asset.authorId = asset.author.id;
  //   this.assetService.updateAsset(asset).subscribe(
  //     () => {
  //       this.dialogRef.close();
  //       this.router.navigate(['/catalog', asset.id]);
  //       this.notify.success('Updated Successful');
  //     },
  //     (error) => {
  //       this.notify.error(error);
  //     }
  //   );
  // }

  ngOnDestroy() {
    this.subs.forEach(sub => {
      sub.unsubscribe();
    });
  }
}
