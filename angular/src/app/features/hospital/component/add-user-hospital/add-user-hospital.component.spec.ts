import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserHospitalComponent } from './add-user-hospital.component';

describe('AddUserHospitalComponent', () => {
  let component: AddUserHospitalComponent;
  let fixture: ComponentFixture<AddUserHospitalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddUserHospitalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddUserHospitalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
