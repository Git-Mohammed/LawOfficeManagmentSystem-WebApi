import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JudgmentsListPage } from './judgments-list-page';

describe('JudgmentsListPage', () => {
  let component: JudgmentsListPage;
  let fixture: ComponentFixture<JudgmentsListPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JudgmentsListPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JudgmentsListPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
