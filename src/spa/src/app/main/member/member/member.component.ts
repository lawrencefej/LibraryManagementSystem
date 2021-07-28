import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { stateValidator } from 'src/app/shared/validators/state.validator';
import { State } from 'src/app/_models/state';
import { User } from 'src/app/_models/user';
import { MemberService } from 'src/app/_services/member.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { StateService } from 'src/app/_services/state.service';

@Component({
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.css']
})
export class MemberComponent implements OnInit {
  stateControl = new FormControl('', Validators.compose([Validators.required, stateValidator]));
  memberForm!: FormGroup;
  member!: User;
  showRevert = false;
  options: string[] = this.stateService.getStates();
  filteredStates$: Observable<string[]> = of([]);

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
    ],
    email: [
      { type: 'required', message: 'Email is required' },
      { type: 'email', message: 'Please enter a valid Email' }
    ],
    phoneNumber: [{ type: 'required', message: 'Phone Number is required' }],
    address: [
      { type: 'required', message: 'Address is required' },
      {
        type: 'maxlength',
        message: 'Address cannot be more than 100 characters'
      }
    ],
    state: [
      { type: 'required', message: 'State is required' },
      { type: 'stateValidator', message: 'Please select a valid state' }
    ],
    stateControl: [
      { type: 'required', message: 'State is required' },
      { type: 'stateValidator', message: 'Please select a valid state' }
    ],
    city: [{ type: 'required', message: 'City is required' }],
    zipcode: [
      { type: 'required', message: 'Zipcode is required' },
      { type: 'pattern', message: 'Please enter a valid Zipcode' }
    ],
    gender: [{ type: 'required', message: 'Gender is required' }]
  };

  constructor(
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: User,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<MemberComponent>,
    private dialog: MatDialog,
    private stateService: StateService,
    public notify: NotificationService,
    private memberService: MemberService
  ) {}

  ngOnInit(): void {
    this.createMemberForm();
    this.filteredStates$ = this.stateControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filterStates(value))
    );
    this.isUpdate();
  }

  updateState(value: any): void {
    this.memberForm.markAsDirty();
    this.memberForm.controls.state.setValue(value.source.value);
  }
  private _filterStates(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  isUpdate(): void {
    if (this.data) {
      this.populateForm(this.data);
      this.member = this.data;
      this.showRevert = true;
    } else {
      this.createMemberForm();
    }
  }

  revert(): void {
    this.populateForm(this.member);
  }

  createMemberForm(): void {
    this.memberForm = this.fb.group({
      id: new FormControl(0),
      firstName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(25)])),
      email: new FormControl('', Validators.compose([Validators.required, Validators.email])),
      phoneNumber: new FormControl('', Validators.compose([Validators.required])),
      address: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(100)])),
      state: new FormControl('', Validators.compose([Validators.required, stateValidator])),
      city: new FormControl('', Validators.compose([Validators.required])),
      zipcode: new FormControl('', Validators.compose([Validators.required, Validators.pattern('^[0-9]{5}$')])),
      gender: new FormControl('', Validators.required)
    });
  }

  populateForm(member: User): void {
    this.memberForm = this.fb.group({
      id: new FormControl(member.id),
      firstName: new FormControl(member.firstName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      lastName: new FormControl(member.lastName, Validators.compose([Validators.required, Validators.maxLength(25)])),
      email: new FormControl(member.email, Validators.compose([Validators.required, Validators.email])),
      phoneNumber: new FormControl(member.phoneNumber, Validators.compose([Validators.required])),
      address: new FormControl(member.address, Validators.compose([Validators.required, Validators.maxLength(100)])),
      state: new FormControl(member.state, Validators.compose([Validators.required, stateValidator])),
      city: new FormControl(member.city, Validators.compose([Validators.required])),
      zipcode: new FormControl(
        member.zipcode,
        Validators.compose([Validators.required, Validators.pattern('^[0-9]{5}$')])
      ),
      gender: new FormControl(member.gender, Validators.required)
    });
    this.stateControl.setValue(member.state);
  }

  displayFn(state?: State): string {
    return state ? state.name : undefined;
  }

  closeDialog(): void {
    if (this.memberForm.dirty) {
      this.notify.discardDialog('Are you sure you want to discard these changes');
    } else {
      this.dialog.closeAll();
    }
  }

  onSubmit(): void {
    if (this.memberForm.controls.id.value) {
      this.updateMember(this.memberForm.value);
    } else {
      this.addMember(this.memberForm.value);
    }
  }

  addMember(member: User): void {
    this.memberService.AddMember(member).subscribe(
      (createdMember: User) => {
        member = createdMember;
        this.dialogRef.close();
        this.router.navigate(['/members', member.id]);
        this.notify.success('Item Added Successfully');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  updateMember(member: User): void {
    this.memberService.updateMember(member).subscribe(
      () => {
        this.dialogRef.close();
        this.router.navigate(['/members', member.id]);
        this.notify.success('Updated Successful');
      },
      error => {
        this.notify.error(error);
      }
    );
  }
}
