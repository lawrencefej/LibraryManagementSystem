import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SearchLibraryCardComponent } from './search-library-card.component';

describe('SearchLibraryCardComponent', () => {
  let component: SearchLibraryCardComponent;
  let fixture: ComponentFixture<SearchLibraryCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SearchLibraryCardComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchLibraryCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
