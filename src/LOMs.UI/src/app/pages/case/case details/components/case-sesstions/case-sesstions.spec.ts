import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaseSesstions } from './case-sesstions';

describe('CaseSesstions', () => {
  let component: CaseSesstions;
  let fixture: ComponentFixture<CaseSesstions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseSesstions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaseSesstions);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
