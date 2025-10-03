import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { courtResolverResolver } from './court-resolver-resolver';

describe('courtResolverResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => courtResolverResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
