import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryAssetDetailCheckoutListComponent } from './library-asset-detail-checkout-list.component';

describe('LibraryAssetDetailCheckoutListComponent', () => {
  let component: LibraryAssetDetailCheckoutListComponent;
  let fixture: ComponentFixture<LibraryAssetDetailCheckoutListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryAssetDetailCheckoutListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryAssetDetailCheckoutListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
