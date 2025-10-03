import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddJudgmentComponent } from './add-judgment-component';

describe('AddJudgmentComponent', () => {
  let component: AddJudgmentComponent;
  let fixture: ComponentFixture<AddJudgmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddJudgmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddJudgmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
