import { Component, EventEmitter, OnDestroy, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { LibraryCardForDetailedDto } from 'src/dto/models';
import { SearchService } from '../../services/search.service';

@Component({
  selector: 'lms-library-card-basic-search',
  templateUrl: './library-card-basic-search.component.html',
  styleUrls: ['./library-card-basic-search.component.css']
})
export class LibraryCardBasicSearchComponent implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Output()
  cards = new EventEmitter<LibraryCardForDetailedDto[]>();

  // TODO add pattern validation
  // TODO add string validation
  cardSearchForm = new FormControl('', Validators.required);
  validationMessages = validationMessages;

  constructor(private readonly searchService: SearchService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  clearForm(): void {
    this.cardSearchForm.reset();
  }

  cardSearch(cardNumber: string): void {
    this.searchService
      .getCardByCardNumber(cardNumber)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(returnCard => this.cards.emit([returnCard]));
  }
}
