import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { MemberService } from 'src/app/_services/member.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-advanced-search',
  templateUrl: './member-advanced-search.component.html',
  styleUrls: ['./member-advanced-search.component.css']
})
export class MemberAdvancedSearchComponent implements OnInit {
  searchForm = new FormGroup({
    firstName: new FormControl('', Validators.required),
    lastName: new FormControl('', Validators.required),
    email: new FormControl('', Validators.compose([Validators.required, Validators.email]))
  });

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
    ]
  };

  members: User[] = [];

  constructor(private memberService: MemberService, private notify: NotificationService) {}

  ngOnInit() {}

  onSubmit() {
    if (this.searchForm.valid) {
      this.memberService.advancedMemberSearch(this.searchForm.value).subscribe(
        (members: User[]) => {
          if (members.length > 0) {
            this.members = members;
            this.searchForm.reset();
            this.searchForm.controls.firstName.setErrors(null);
            this.searchForm.controls.lastName.setErrors(null);
            this.searchForm.controls.email.setErrors(null);
          } else {
            this.notify.warn('No members were found. Please refine your search and try again');
          }
        },
        error => {
          this.notify.error(error);
        }
      );
    }
  }

  onClear() {
    this.searchForm.reset();
  }
}
