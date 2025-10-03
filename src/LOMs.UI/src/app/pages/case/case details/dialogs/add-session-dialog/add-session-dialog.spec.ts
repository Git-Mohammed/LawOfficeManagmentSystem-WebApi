import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSessionDialog } from './add-session-dialog';

describe('AddSessionDialog', () => {
  let component: AddSessionDialog;
  let fixture: ComponentFixture<AddSessionDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddSessionDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddSessionDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
