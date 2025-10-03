import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaseContract } from './case-contract';

describe('CaseContract', () => {
  let component: CaseContract;
  let fixture: ComponentFixture<CaseContract>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseContract]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaseContract);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
