import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddPoaDialog } from './add-poa';

describe('AddPoaDialog', () => {
  let component: AddPoaDialog;
  let fixture: ComponentFixture<AddPoaDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddPoaDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddPoaDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
