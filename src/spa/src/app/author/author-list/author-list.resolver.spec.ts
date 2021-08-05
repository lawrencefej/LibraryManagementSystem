import { TestBed } from '@angular/core/testing';

import { AuthorListResolver } from './author-list.resolver';

describe('AuthorListResolver', () => {
  let resolver: AuthorListResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(AuthorListResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
