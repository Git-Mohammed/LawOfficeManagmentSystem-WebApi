import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { childAuthGuardGuard } from './child-auth-guard-guard';

describe('childAuthGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => childAuthGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
