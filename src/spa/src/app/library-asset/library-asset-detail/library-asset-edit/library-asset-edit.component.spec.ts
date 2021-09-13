import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryAssetEditComponent } from './library-asset-edit.component';

describe('LibraryAssetEditComponent', () => {
  let component: LibraryAssetEditComponent;
  let fixture: ComponentFixture<LibraryAssetEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryAssetEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryAssetEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
