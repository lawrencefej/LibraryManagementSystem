import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { libraryAssetValidationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryAssetForDetailedDto } from 'src/dto/models';
import { LibraryAssetService } from '../../services/library-asset.service';

@Component({
  selector: 'lms-library-asset-edit',
  templateUrl: './library-asset-edit.component.html',
  styleUrls: ['./library-asset-edit.component.css']
})
export class LibraryAssetEditComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  // TODO Cleanup
  @Input() asset!: LibraryAssetForDetailedDto;
  // @Input() states!: StateDto[];
  @Output() assetChange = new EventEmitter<LibraryAssetForDetailedDto>();
  @Output() closeTab = new EventEmitter<void>();
  @Output() isFormDirty = new EventEmitter<boolean>();

  assetForm!: FormGroup;
  // filteredStates?: Observable<StateDto[]> = of([]);
  validationMessages = libraryAssetValidationMessages;
  // genders = MemberGenderDto;
  serverValidationErrors: string[] = [];

  constructor(
    private readonly assetService: LibraryAssetService,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {}

  cancelEdit(): void {
    this.closeTab.emit();
  }

  populateForm(asset: LibraryAssetForDetailedDto): void {
    this.assetForm = this.fb.group({
      id: new FormControl(asset.id),
      title: new FormControl(asset.title, Validators.compose([Validators.required, Validators.maxLength(25)])),
      author: new FormControl(asset.authors, Validators.compose([Validators.required])),
      // authorId: new FormControl(asset.author.id, Validators.compose([Validators.required])),
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
      categories: new FormControl(asset.categories, Validators.required),
      assetType: new FormControl(asset.assetType, Validators.required),
      // statusId: new FormControl(asset.assetType.id, Validators.required),
      isbn: new FormControl({ value: asset.isbn, disabled: true }, Validators.required)
    });
  }
}
