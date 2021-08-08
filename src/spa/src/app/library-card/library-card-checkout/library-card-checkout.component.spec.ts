/* tslint:disable:no-unused-variable */
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { LibraryCardCheckoutComponent } from './library-card-checkout.component';

describe('LibraryCardCheckoutComponent', () => {
  let component: LibraryCardCheckoutComponent;
  let fixture: ComponentFixture<LibraryCardCheckoutComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [LibraryCardCheckoutComponent]
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryCardCheckoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
