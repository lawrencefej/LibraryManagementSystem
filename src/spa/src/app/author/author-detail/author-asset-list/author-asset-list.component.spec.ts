import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorAssetListComponent } from './author-asset-list.component';

describe('AuthorAssetListComponent', () => {
  let component: AuthorAssetListComponent;
  let fixture: ComponentFixture<AuthorAssetListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthorAssetListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorAssetListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
