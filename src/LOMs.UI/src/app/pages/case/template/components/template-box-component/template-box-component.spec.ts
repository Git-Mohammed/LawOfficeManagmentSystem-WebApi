import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateBoxComponent } from './template-box-component';

describe('TemplateBoxComponent', () => {
  let component: TemplateBoxComponent;
  let fixture: ComponentFixture<TemplateBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplateBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TemplateBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
