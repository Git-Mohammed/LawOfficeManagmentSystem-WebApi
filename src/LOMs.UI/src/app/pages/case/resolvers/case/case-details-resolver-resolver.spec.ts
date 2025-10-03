import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { caseDetailsResolverResolver } from './case-details-resolver-resolver';

describe('caseDetailsResolverResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => caseDetailsResolverResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
