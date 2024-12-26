import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportWardExcelComponent } from './import-ward-excel.component';

describe('ImportWardExcelComponent', () => {
  let component: ImportWardExcelComponent;
  let fixture: ComponentFixture<ImportWardExcelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImportWardExcelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ImportWardExcelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
