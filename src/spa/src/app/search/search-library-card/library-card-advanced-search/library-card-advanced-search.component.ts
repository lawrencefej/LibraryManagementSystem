import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { LibraryCardForDetailedDto } from 'src/dto/models';
import { LibraryCardAdvancedSearchViewModel } from '../../models/library-card-advanced-search-view-model';
import { SearchService } from '../../services/search.service';

@Component({
  selector: 'lms-library-card-advanced-search',
  templateUrl: './library-card-advanced-search.component.html',
  styleUrls: ['./library-card-advanced-search.component.css']
})
export class LibraryCardAdvancedSearchComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Output()
  cards = new EventEmitter<LibraryCardForDetailedDto[]>();

  advancedSearchForm = new FormGroup({
    firstName: new FormControl('', Validators.maxLength(25)),
    lastName: new FormControl('', Validators.maxLength(25)),
    email: new FormControl('', Validators.compose([Validators.email, Validators.maxLength(100)])),
    phoneNumber: new FormControl('', Validators.compose([Validators.maxLength(15)])),
    dateOfBirth: new FormControl(''),
    zipcode: new FormControl('', Validators.compose([Validators.pattern('^[0-9]{5}$')]))
  });
  validationMessages = validationMessages;

  constructor(private readonly searchService: SearchService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {}

  advancedSearch(searchDetails: LibraryCardAdvancedSearchViewModel): void {
    this.searchService
      .advancedCardSearch(searchDetails)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(returnCards => this.cards.emit(returnCards));
  }

  resetForm(): void {
    this.advancedSearchForm.reset();
  }
}
