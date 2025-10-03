import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTemplateDialog } from './add-template-dialog';

describe('AddTemplateDialog', () => {
  let component: AddTemplateDialog;
  let fixture: ComponentFixture<AddTemplateDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddTemplateDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddTemplateDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
