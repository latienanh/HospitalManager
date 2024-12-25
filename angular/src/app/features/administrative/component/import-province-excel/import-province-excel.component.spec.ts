import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportProvinceExcelComponent } from './import-province-excel.component';

describe('ImportProvinceExcelComponent', () => {
  let component: ImportProvinceExcelComponent;
  let fixture: ComponentFixture<ImportProvinceExcelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImportProvinceExcelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ImportProvinceExcelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
