import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { stateValidator } from 'src/app/shared/validators/state.validator';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryCardForDetailedDto, LibraryCardForUpdate, MemberGenderDto, StateDto } from 'src/dto/models';
import { LibraryCardService } from '../services/library-card.service';

@Component({
  selector: 'lms-library-card-edit',
  templateUrl: './library-card-edit.component.html',
  styleUrls: ['./library-card-edit.component.css']
})
export class LibraryCardEditComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input() card!: LibraryCardForDetailedDto;
  @Input() states: StateDto[] = [];

  @Output() cardChange = new EventEmitter<LibraryCardForDetailedDto>();
  @Output() closeTab = new EventEmitter<void>();
  @Output() isFormDirty = new EventEmitter<boolean>();

  cardForm!: FormGroup;
  filteredStates?: Observable<StateDto[]> = of([]);
  validationMessages = validationMessages;
  genders = MemberGenderDto;
  serverValidationErrors: string[] = [];

  constructor(
    private readonly cardService: LibraryCardService,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService
  ) {}

  ngOnInit(): void {
    this.populateForm(this.card);
    this.filteredStates = this.cardForm.get('address.state')?.valueChanges.pipe(
      startWith(''),
      map(s => (s ? this.filterStates(s) : this.states.slice()))
    );
    this.cardForm.valueChanges.pipe(takeUntil(this.unsubscribe)).subscribe(() => {
      this.isFormDirty.emit(this.cardForm.dirty);
    });
    this.setStateId();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  cancelEdit(): void {
    this.closeTab.emit();
  }

  revert(): void {
    this.notify
      .confirm('Are you sure you want to discard these changes?')
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(response => {
        if (response) {
          this.populateForm(this.card);
          this.isFormDirty.emit(false);
        }
      });
  }

  editCard(card: LibraryCardForUpdate): void {
    this.cardService
      .updateCard(card)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        returnCard => {
          this.isFormDirty.emit(false);
          this.closeTab.emit();
          this.notify.success('Card updated successfully');
          this.cardChange.emit(returnCard);
        },
        error => {
          this.serverValidationErrors = error;
        }
      );
  }

  displayStateName(state: StateDto): string {
    return state.name;
  }

  private setStateId(): void {
    this.cardForm
      .get('address.state')
      ?.valueChanges.pipe(takeUntil(this.unsubscribe))
      .subscribe(() => {
        const newState: StateDto = this.cardForm.get('address.state')?.value;

        if (newState) {
          this.cardForm.get('address.stateId')?.setValue(newState.id);
        }
      });
  }

  private populateForm(card: LibraryCardForDetailedDto): void {
    this.cardForm = this.fb.group({
      id: new FormControl(card.id),
      firstName: new FormControl(card.firstName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl(card.lastName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      email: new FormControl(
        card.email,
        Validators.compose([Validators.required, Validators.email, Validators.maxLength(100)])
      ),
      phoneNumber: new FormControl(
        card.phoneNumber,
        Validators.compose([Validators.required, Validators.maxLength(15)])
      ),
      gender: new FormControl(card.gender, Validators.required),

      address: new FormGroup({
        id: new FormControl(card.address.id),
        street: new FormControl(
          card.address.street,
          Validators.compose([Validators.required, Validators.maxLength(50)])
        ),
        city: new FormControl(card.address.city, Validators.compose([Validators.required, Validators.maxLength(25)])),
        zipcode: new FormControl(
          card.address.zipcode,
          Validators.compose([Validators.required, Validators.pattern('^[0-9]{5}$')])
        ),
        stateId: new FormControl(card.address.stateId, Validators.compose([Validators.required])),
        state: new FormControl(card.address.state, Validators.compose([Validators.required, stateValidator]))
      })
    });
  }

  private filterStates(value: any): StateDto[] {
    let filterValue: string;

    value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

    return this.states.filter(state => state.name.toLowerCase().includes(filterValue));
  }
}
