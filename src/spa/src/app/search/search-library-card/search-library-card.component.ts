import { Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { validationMessages } from 'src/app/shared/validators/validator.constants';
import { SessionService } from 'src/app/_services/session.service';
import { LibraryCardForDetailedDto } from 'src/dto/models';

@Component({
  templateUrl: './search-library-card.component.html',
  styleUrls: ['./search-library-card.component.css']
})
export class SearchLibraryCardComponent implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  cards: LibraryCardForDetailedDto[] = [];
  validationMessages = validationMessages;
  showResult = false;

  constructor(private readonly sessionService: SessionService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  getSearchResult(cards: LibraryCardForDetailedDto[]): void {
    this.cards = cards;
    this.showResult = true;
  }

  tabClicked(): void {
    this.showResult = false;
    this.sessionService.resetLogoutTimer();
  }
}
