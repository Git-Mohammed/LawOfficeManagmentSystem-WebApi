import { TestBed } from '@angular/core/testing';

import { JudgmentService } from './judgment-service';

describe('JudgmentService', () => {
  let service: JudgmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JudgmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
