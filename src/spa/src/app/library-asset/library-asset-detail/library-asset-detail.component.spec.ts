import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryAssetDetailComponent } from './library-asset-detail.component';

describe('LibraryAssetDetailComponent', () => {
  let component: LibraryAssetDetailComponent;
  let fixture: ComponentFixture<LibraryAssetDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryAssetDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryAssetDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
