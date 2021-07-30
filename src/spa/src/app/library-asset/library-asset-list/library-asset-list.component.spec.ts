import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryAssetListComponent } from './library-asset-list.component';

describe('LibraryAssetListComponent', () => {
  let component: LibraryAssetListComponent;
  let fixture: ComponentFixture<LibraryAssetListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryAssetListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryAssetListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
