import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MangeEmployeePage } from './mange-employee-page';

describe('MangeEmployeePage', () => {
  let component: MangeEmployeePage;
  let fixture: ComponentFixture<MangeEmployeePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MangeEmployeePage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MangeEmployeePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
