import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorAssetsComponent } from './author-assets.component';

describe('AuthorAssetsComponent', () => {
  let component: AuthorAssetsComponent;
  let fixture: ComponentFixture<AuthorAssetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthorAssetsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorAssetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
