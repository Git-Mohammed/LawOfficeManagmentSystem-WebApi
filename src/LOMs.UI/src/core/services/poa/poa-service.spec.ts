import { TestBed } from '@angular/core/testing';

import { POAService } from './poa-service';

describe('POAService', () => {
  let service: POAService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(POAService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
