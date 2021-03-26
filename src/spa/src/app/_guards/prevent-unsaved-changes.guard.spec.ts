import { TestBed, inject, waitForAsync } from '@angular/core/testing';

import { PreventUnsavedChangesGuard } from './prevent-unsaved-changes.guard';

describe('PerventUnsavedChangesGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PreventUnsavedChangesGuard]
    });
  });

  it('should ...', inject([PreventUnsavedChangesGuard], (guard: PreventUnsavedChangesGuard) => {
    expect(guard).toBeTruthy();
  }));
});
