import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { User } from 'src/app/_models/user';
import { MemberService } from 'src/app/_services/member.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { MemberComponent } from '../member/member.component';

@Component({
  selector: 'lms-member-search',
  templateUrl: './member-search.component.html',
  styleUrls: ['./member-search.component.css']
})
export class MemberSearchComponent implements OnInit {
  searchForm: FormGroup;
  member: User;
  validationMessages = {
    cardNumber: [
      { type: 'maxlength', message: 'Card number cannot be more than 10 digits' },
      { type: 'pattern', message: 'Please enter a valid card Number' }
    ]
  };

  constructor(
    private memberService: MemberService,
    private fb: FormBuilder,
    private notify: NotificationService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.createSearchForm();
  }

  createSearchForm(): void {
    this.searchForm = this.fb.group({
      cardNumber: new FormControl('', Validators.compose([Validators.maxLength(10), Validators.pattern('^[0-9]*$')]))
    });
  }

  onSubmit(): void {
    this.memberService.getMemberByCardNumber(this.searchForm.controls.cardNumber.value).subscribe(
      (member: User) => {
        if (member != null) {
          this.member = member;
          this.searchForm.reset();
          this.searchForm.controls.cardNumber.setErrors(null);
          return;
        }
        this.notify.error('This card number does not exist');
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  openMemberDialog(): void {
    this.dialog.open(MemberComponent, {
      width: '640px',
      disableClose: true
    });
  }

  selectMember(): void {
    this.router.navigate(['/members/', this.member.id]);
  }

  fullList(): void {
    this.router.navigate(['/members']);
  }
}
