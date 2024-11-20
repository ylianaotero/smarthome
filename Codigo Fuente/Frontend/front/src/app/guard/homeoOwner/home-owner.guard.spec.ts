import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { homeOwnerGuard } from './home-owner.guard';

describe('homeOwnerGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => homeOwnerGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
