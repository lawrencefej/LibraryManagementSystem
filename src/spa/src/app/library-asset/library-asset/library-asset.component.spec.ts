import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryAssetComponent } from './library-asset.component';

describe('LibraryAssetComponent', () => {
  let component: LibraryAssetComponent;
  let fixture: ComponentFixture<LibraryAssetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryAssetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
