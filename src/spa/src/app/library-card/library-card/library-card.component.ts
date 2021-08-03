import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, of, Subject } from 'rxjs';
import { map, startWith, takeUntil } from 'rxjs/operators';
import { stateValidator } from 'src/app/shared/validators/state.validator';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { AppConfigService, MinMaxDates } from 'src/app/_services/app-config.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { StateService } from 'src/app/_services/state.service';
import { LibraryCardForCreationDto, MemberGenderDto, StateDto } from 'src/dto/models';
import { LibraryCardService } from '../services/library-card.service';

@Component({
  templateUrl: './library-card.component.html',
  styleUrls: ['./library-card.component.css']
})
export class LibraryCardComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  cardForm!: FormGroup;
  filteredStates?: Observable<StateDto[]> = of([]);
  genders = MemberGenderDto;
  minMaxDates!: MinMaxDates;
  serverValidationErrors: string[] = [];
  states: StateDto[] = [];
  validationMessages = validationMessages;

  constructor(
    private readonly appConfig: AppConfigService,
    private readonly dialog: MatDialog,
    private readonly cardService: LibraryCardService,
    private readonly dialogRef: MatDialogRef<LibraryCardComponent>,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService,
    private readonly router: Router,
    private readonly stateService: StateService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.createForm();

    // TODO make this a service instead
    this.stateService
      .getStatesObject()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(state => (this.states = state));
    this.watchStateChanges();
    this.setStateId();
    this.minMaxDates = this.appConfig.calculateMinMaxDate();
  }

  addCard(card: LibraryCardForCreationDto): void {
    this.cardService
      .AddCard(card)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        returnCard => {
          this.dialogRef.close();
          this.router.navigateByUrl(`/library-card/cards/${returnCard.id}`);
          this.notify.success('Card update successfully');
        },
        error => {
          this.serverValidationErrors = error;
        }
      );
  }

  cancel(): void {
    if (this.cardForm.dirty) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  displayStateName(state: StateDto): string {
    return state.name;
  }

  private watchStateChanges(): void {
    this.filteredStates = this.cardForm.get('address.state')?.valueChanges.pipe(
      startWith(''),
      map(s => (s ? this.filterStates(s) : this.states.slice()))
    );
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

  private filterStates(value: any): StateDto[] {
    let filterValue: string;

    value.name ? (filterValue = value.name.toLowerCase()) : (filterValue = value.toLowerCase());

    return this.states.filter(state => state.name.toLowerCase().includes(filterValue));
  }

  private createForm(): void {
    this.cardForm = this.fb.group({
      firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      email: new FormControl(
        '',
        Validators.compose([Validators.required, Validators.email, Validators.maxLength(100)])
      ),
      phoneNumber: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(15)])),
      gender: new FormControl('', Validators.required),
      dateOfBirth: new FormControl('', Validators.required),

      address: new FormGroup({
        street: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(50)])),
        city: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
        zipcode: new FormControl('', Validators.compose([Validators.required, Validators.pattern('^[0-9]{5}$')])),
        stateId: new FormControl('', Validators.compose([Validators.required])),
        state: new FormControl('', Validators.compose([Validators.required, stateValidator]))
      })
    });
  }
}
