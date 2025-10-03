import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionButton } from './section-button';

describe('SectionButton', () => {
  let component: SectionButton;
  let fixture: ComponentFixture<SectionButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionButton]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SectionButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
