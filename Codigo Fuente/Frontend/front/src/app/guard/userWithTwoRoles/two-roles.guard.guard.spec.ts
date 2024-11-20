import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { twoRolesGuardGuard } from './two-roles.guard.guard';

describe('twoRolesGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => twoRolesGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
