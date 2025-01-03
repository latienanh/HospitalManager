import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateHospitalComponent } from './update-hospital.component';

describe('UpdateHospitalComponent', () => {
  let component: UpdateHospitalComponent;
  let fixture: ComponentFixture<UpdateHospitalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateHospitalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdateHospitalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
