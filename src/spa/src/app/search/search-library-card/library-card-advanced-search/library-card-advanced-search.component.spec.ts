import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryCardAdvancedSearchComponent } from './library-card-advanced-search.component';

describe('LibraryCardAdvancedSearchComponent', () => {
  let component: LibraryCardAdvancedSearchComponent;
  let fixture: ComponentFixture<LibraryCardAdvancedSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryCardAdvancedSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryCardAdvancedSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
