import { TestBed } from '@angular/core/testing';

import { DashboardPanelResolver } from './dashboard-panel.resolver';

describe('DashboardPanelResolver', () => {
  let resolver: DashboardPanelResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(DashboardPanelResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
