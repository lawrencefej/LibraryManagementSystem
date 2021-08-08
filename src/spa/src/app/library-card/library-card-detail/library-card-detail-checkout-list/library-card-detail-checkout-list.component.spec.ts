/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { LibraryCardDetailCheckoutListComponent } from './library-card-detail-checkout-list.component';

describe('LibraryCardDetailCheckoutListComponent', () => {
  let component: LibraryCardDetailCheckoutListComponent;
  let fixture: ComponentFixture<LibraryCardDetailCheckoutListComponent>;

  beforeEach(
    waitForAsync(() => {
      TestBed.configureTestingModule({
        declarations: [LibraryCardDetailCheckoutListComponent]
      }).compileComponents();
    })
  );

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryCardDetailCheckoutListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
