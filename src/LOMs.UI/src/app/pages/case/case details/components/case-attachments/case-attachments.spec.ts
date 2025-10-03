import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CaseAttachments } from './case-attachments';

describe('CaseAttachments', () => {
  let component: CaseAttachments;
  let fixture: ComponentFixture<CaseAttachments>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CaseAttachments]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CaseAttachments);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
