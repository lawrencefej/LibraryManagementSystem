import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryCardForDetailedDto } from 'src/dto/models/library-card-for-detailed-dto';
import { LibraryCardComponent } from '../library-card/library-card.component';
import { LibraryCardService } from '../services/library-card.service';

@Component({
  templateUrl: './library-card-search.component.html',
  styleUrls: ['./library-card-search.component.css']
})
export class LibraryCardSearchComponent implements OnInit {
  card?: LibraryCardForDetailedDto;
  searchForm!: FormGroup;
  // TODO move to central
  validationMessages = {
    cardNumber: [
      {
        type: 'maxlength',
        message: 'Card number cannot be more than 18 digits'
      }
      // { type: 'pattern', message: 'Please enter a valid card Number' },
    ]
  };

  constructor(
    private readonly cardService: LibraryCardService,
    private readonly fb: FormBuilder,
    private readonly notify: NotificationService,
    private readonly router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.createSearchForm();
  }

  onSubmit(): void {
    this.cardService.getCardByCardNumber(this.searchForm.controls.cardNumber.value).subscribe(
      card => {
        if (card != null) {
          this.card = card;
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

  createSearchForm(): void {
    this.searchForm = this.fb.group({
      cardNumber: new FormControl(
        '',
        Validators.compose([
          Validators.maxLength(18)
          // TODO fix the pattern validator
          // Validators.pattern('^[0-9]*$'),
        ])
      )
    });
  }

  openCardDialog(): void {
    this.dialog.open(LibraryCardComponent, {
      width: '640px',
      disableClose: true
    });
  }

  selectCard(): void {
    // tslint:disable-next-line: no-non-null-assertion
    this.router.navigate(['/library-card/cards', this.card!.id]);
  }

  fillList(): void {
    this.router.navigate(['/library-card/cards']);
  }
}
