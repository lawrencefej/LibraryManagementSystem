import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryCardBasicSearchComponent } from './library-card-basic-search.component';

describe('LibraryCardBasicSearchComponent', () => {
  let component: LibraryCardBasicSearchComponent;
  let fixture: ComponentFixture<LibraryCardBasicSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryCardBasicSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryCardBasicSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
