import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasePoa } from './case-poa';

describe('CasePoa', () => {
  let component: CasePoa;
  let fixture: ComponentFixture<CasePoa>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasePoa]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasePoa);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
