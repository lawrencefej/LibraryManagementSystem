import { TestBed } from '@angular/core/testing';

import { AuthorDetailResolver } from './author-detail.resolver';

describe('AuthorDetailResolver', () => {
  let resolver: AuthorDetailResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(AuthorDetailResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
