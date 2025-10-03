import { TestBed } from '@angular/core/testing';

import { AuthManagement } from './auth-management';

describe('AuthManagement', () => {
  let service: AuthManagement;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthManagement);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
