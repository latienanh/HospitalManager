import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingDistrictRequest, GetPagingWardRequest } from '@proxy/dtos/request/get-paging';
import { DistrictDto, GetPagingResponse, ProvinceDto, WardDto } from '@proxy/dtos/response';
import { PatientService } from '@proxy/patient-manager/services';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Subject } from 'rxjs';
import { ErrorResponse } from 'src/app/core/models/error-response.model';
import { VisibleAdd } from 'src/app/features/administrative/models/visible';

@Component({
  selector: 'app-add-patient',
  templateUrl: './add-patient.component.html',
  styleUrl: './add-patient.component.scss'
})
export class AddPatientComponent implements OnInit,OnDestroy {
  @Input() isVisibleAddPatient: VisibleAdd;
  @Output() visibilityChange = new EventEmitter<VisibleAdd>();

  private destroy$ = new Subject<void>();
  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };
  districts: GetPagingResponse<DistrictDto> = {
    data: [],
    totalPage: 0,
  };
  wards: GetPagingResponse<WardDto> = {
    data: [],
    totalPage: 0,
  };
  patientAddForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private PatientService: PatientService,
    private notification: NzNotificationService,
    private districtService: DistrictService,
    private provinceService: ProvinceService,
    private wardService: WardService
  ) {}

  ngOnInit(): void {  
    this.patientAddForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      provinceCode: [null, [Validators.required]],
      districtCode: [null, [Validators.required]],
      wardCode:[null, [Validators.required]],
      address:['', [Validators.required]],
    });
    this.loadProvinces();
    this.patientAddForm.get('provinceCode').valueChanges.subscribe(value => {
      this.patientAddForm.get('districtCode').setValue(null);
      this.loadDistricts(value);
    });
    this.patientAddForm.get('districtCode').valueChanges.subscribe(value => {
      this.loadWards(this.patientAddForm.value.provinceCode,value);
    });
  
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
   loadWards(provinceCode:number,districtCode:number) {
      if(provinceCode==null||districtCode==null)
        {
          return
        }
      const query: GetPagingWardRequest = {
        index: 0,
        size: 1000,
        provinceCode:provinceCode,
        districtCode:districtCode
      };
      this.wardService.getWardDapperList(query).subscribe({
        next: response => {
          console.log(response)
          this.wards = response;
        },
        error: err => {
          console.log(err);
        },
      });
    }
 loadDistricts(provinceCode: number) {
  if(provinceCode==null)
    {
      return
    }
    const query: GetPagingDistrictRequest = {
      index: 0,
      size: 1000,
      provinceCode: provinceCode,
    };

    this.districtService.getDistrictDapperList(query).subscribe({
      next: response => {
        this.districts = response;
      },
      error: err => {
        console.log(err);
      },
    });
  }
  loadProvinces() {
    const query: BaseGetPagingRequest = {
      index: 0,
      size: 1000,
    };

    this.provinceService.getProvinceDapperList(query).subscribe({
      next: response => {
        this.provinces = response;
      },
      error: err => {
        console.log(err);
      },
    });
  }

  submitForm(): void {
    if (this.patientAddForm.valid) {
      const formValue = this.patientAddForm.value;
      const PatientData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
        provinceCode: Number(formValue.provinceCode)
      };

      this.PatientService.create(PatientData).subscribe({
        next: response => {
          console.log('Tạo tỉnh thành công:', response);
          this.notification.create('success', 'Thêm thành công', `${response.name}`);
          this.patientAddForm.reset();
          this.visibilityChange.emit({ showAddForm: false, addStatus: true });
        },
        error: (err: ErrorResponse) => {
          this.notification.create('error', 'Lỗi', `${err.error.error.data.message}`);
        },
      });
    } else {
      Object.values(this.patientAddForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  handleCancel() {
    this.visibilityChange.emit({ addStatus: false, showAddForm: false });
  }
}
