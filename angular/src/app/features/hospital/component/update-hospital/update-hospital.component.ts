import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingDistrictRequest, GetPagingWardRequest } from '@proxy/dtos/request/get-paging';
import { DistrictDto, GetPagingResponse, ProvinceDto, WardDto } from '@proxy/dtos/response';
import { DistrictService, HospitalService, ProvinceService, WardService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Subject, takeUntil } from 'rxjs';
import { ErrorResponse } from 'src/app/core/models/error-response.model';
import { VisibleUpdate } from 'src/app/features/administrative/models/visible';

@Component({
  selector: 'app-update-hospital',
  templateUrl: './update-hospital.component.html',
  styleUrl: './update-hospital.component.scss'
})
export class UpdateHospitalComponent implements OnInit,OnDestroy {
  @Input() id:number;
  @Input() isVisibleUpdateHospital: VisibleUpdate ;
  @Output() visibilityChange = new EventEmitter<VisibleUpdate>();
  handleCancel() {
    this.visibilityChange.emit({ updateStatus: false, showUpdateForm: false });
  }
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
  hospitalUpdateForm = this.fb.group({
    code: this.fb.control('', [Validators.required]),
    name: this.fb.control('', [Validators.required]),
    provinceCode: this.fb.control<string | number | null>(null, [Validators.required]),
    districtCode: this.fb.control<string | number | null>(null, [Validators.required]),
    wardCode:this.fb.control<string | number | null>(null, [Validators.required]),
    address:['', [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private hospitalService: HospitalService,
    private notification: NzNotificationService,
     private provinceService: ProvinceService,
     private districtService: DistrictService,
     private wardService: WardService
  ) {
   
  }
  ngOnInit(): void {
    this.loadProvinces();
    this.hospitalUpdateForm.get('provinceCode').valueChanges.subscribe(value => {
      this.hospitalUpdateForm.get('districtCode').setValue(null);
      this.loadDistricts(value);
    });
    this.hospitalUpdateForm.get('districtCode').valueChanges.subscribe(value => {
      this.loadWards(this.hospitalUpdateForm.value.provinceCode,value);
    });
    this.hospitalService.get(this.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.hospitalUpdateForm.patchValue({
            code: response.code.toString(),
            name: response.name,   
            provinceCode: response.provinceCode,
            districtCode: response.districtCode,
            wardCode: response.wardCode,
            address:response.address
          });
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  loadWards(provinceCode:number|string,districtCode:number|string) {
        if(provinceCode==null||districtCode==null)
          {
            return
          }
        const query: GetPagingWardRequest = {
          index: 0,
          size: 1000,
          provinceCode:Number(provinceCode),
          districtCode:Number(districtCode)
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
  loadDistricts(provinceCode: number|string) {
    console.log(provinceCode)
    if(provinceCode==null)
      {
        return
      }
      const query: GetPagingDistrictRequest = {
        index: 0,
        size: 1000,
        provinceCode:Number(provinceCode),
      };
  
      this.districtService.getDistrictDapperList(query).subscribe({
        next: response => {
          this.districts = response;
          console.log(this.districts);
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
    if (this.hospitalUpdateForm.valid) {
      const formValue = this.hospitalUpdateForm.value;

      const hospitalData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
        provinceCode: Number(formValue.provinceCode),
        districtCode:Number(formValue.districtCode),
        wardCode:Number(formValue.wardCode)
      };

      this.hospitalService.update(this.id,hospitalData).subscribe({
        next: response => {
          console.log('Cập nhật tỉnh thành công:', response);
          this.notification.create(
            'success',
            'Cập nhật thành công',
            `${response.name}`
          );
          this.hospitalUpdateForm.reset();
          this.visibilityChange.emit({  showUpdateForm: false,updateStatus: true });
        },
        error:(err:ErrorResponse)  => {
          console.log(err)
          this.notification.create(
            'error',
            'Lỗi',
            `${err.error.error.data.message}`
          );
        },
      });

     
    } else {
      Object.values(this.hospitalUpdateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
