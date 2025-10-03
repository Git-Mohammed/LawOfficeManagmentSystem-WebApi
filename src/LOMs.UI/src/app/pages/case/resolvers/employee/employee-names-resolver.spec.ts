import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { employeeNamesResolver } from './employee-names-resolver';

describe('employeeNamesResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => employeeNamesResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
