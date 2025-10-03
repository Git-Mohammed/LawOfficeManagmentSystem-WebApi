import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplatsPage } from './templats-page';

describe('TemplatsPage', () => {
  let component: TemplatsPage;
  let fixture: ComponentFixture<TemplatsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TemplatsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TemplatsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
