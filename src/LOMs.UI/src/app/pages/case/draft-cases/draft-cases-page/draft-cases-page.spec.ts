import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DraftCasesPage } from './draft-cases-page';

describe('DraftCasesPage', () => {
  let component: DraftCasesPage;
  let fixture: ComponentFixture<DraftCasesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DraftCasesPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DraftCasesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
