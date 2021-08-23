import { TestBed } from '@angular/core/testing';

import { UserProfileEditGuard } from './user-profile-edit.guard';

describe('UserProfileEditGuard', () => {
  let guard: UserProfileEditGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(UserProfileEditGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
