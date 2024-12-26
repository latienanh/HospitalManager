import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { VisibleUpdate } from '../../models/visible';
import { Subject, takeUntil } from 'rxjs';
import { DistrictDto, GetPagingResponse, ProvinceDto } from '@proxy/dtos/response';
import { FormBuilder, Validators } from '@angular/forms';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { GetPagingDistrictRequest } from '@proxy/dtos/request/get-paging';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { ErrorResponse } from 'src/app/core/models/error-response.model';

@Component({
  selector: 'app-update-ward',
  templateUrl: './update-ward.component.html',
  styleUrl: './update-ward.component.scss'
})
export class UpdateWardComponent implements OnInit,OnDestroy{
  @Input() id:number;
  @Input() isVisibleUpdateWard: VisibleUpdate ;
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
  wardUpdateForm = this.fb.group({
    code: this.fb.control('', [Validators.required]),
    name: this.fb.control('', [Validators.required]),
    administrativeLevel: this.fb.control('', [Validators.required]),
    note: this.fb.control('', [Validators.required]),
    provinceCode: this.fb.control<string | number | null>(null, [Validators.required]),
    districtCode: this.fb.control<string | number | null>(null, [Validators.required])
  });

  constructor(
    private fb: FormBuilder,
    private wardService: WardService,
    private notification: NzNotificationService,
     private provinceService: ProvinceService,
     private districtService: DistrictService
  ) {
   
  }
  ngOnInit(): void {
    this.loadProvinces();
    this.wardUpdateForm.get('provinceCode').valueChanges.subscribe(value => {
      this.wardUpdateForm.get('districtCode').setValue(null);
      this.loadDistricts(value);
    });
    this.wardService.get(this.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.wardUpdateForm.patchValue({
            code: response.code.toString(),
            name: response.name,
            administrativeLevel: response.administrativeLevel,
            note: response.note,
            provinceCode: response.provinceCode,
            districtCode: response.districtCode
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
    if (this.wardUpdateForm.valid) {
      const formValue = this.wardUpdateForm.value;

      const wardData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
        provinceCode: Number(formValue.provinceCode),
        districtCode:Number(formValue.districtCode)
      };

      this.wardService.update(this.id,wardData).subscribe({
        next: response => {
          console.log('Cập nhật tỉnh thành công:', response);
          this.notification.create(
            'success',
            'Cập nhật thành công',
            `${response.name}`
          );
          this.wardUpdateForm.reset();
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
      Object.values(this.wardUpdateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
