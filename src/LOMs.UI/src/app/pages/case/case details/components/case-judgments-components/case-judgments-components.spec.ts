import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaseJudgmentsComponents } from './case-judgments-components';

describe('CaseJudgmentsComponents', () => {
  let component: CaseJudgmentsComponents;
  let fixture: ComponentFixture<CaseJudgmentsComponents>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseJudgmentsComponents]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaseJudgmentsComponents);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
