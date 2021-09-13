import { TestBed } from '@angular/core/testing';

import { AdminListResolver } from './admin-list.resolver';

describe('AdminListResolver', () => {
  let resolver: AdminListResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(AdminListResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
