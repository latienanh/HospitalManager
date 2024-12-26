import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportDistrictExcelComponent } from './import-district-excel.component';

describe('ImportDistrictExcelComponent', () => {
  let component: ImportDistrictExcelComponent;
  let fixture: ComponentFixture<ImportDistrictExcelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImportDistrictExcelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ImportDistrictExcelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
