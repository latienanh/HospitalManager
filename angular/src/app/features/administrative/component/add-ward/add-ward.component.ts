import { Component, EventEmitter, Input, Output, OnInit, OnDestroy } from '@angular/core';
import { VisibleAdd } from '../../models/visible';
import { Subject } from 'rxjs';
import { DistrictDto, GetPagingResponse, ProvinceDto } from '@proxy/dtos/response';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DistrictService, ProvinceService, WardService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { GetPagingDistrictRequest } from '@proxy/dtos/request/get-paging';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { ErrorResponse } from 'src/app/core/models/error-response.model';

@Component({
  selector: 'app-add-ward',
  templateUrl: './add-ward.component.html',
  styleUrl: './add-ward.component.scss'
})
export class AddWardComponent implements OnInit,OnDestroy{
  @Input() isVisibleAddWard: VisibleAdd;
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
  wardAddForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private WardService: WardService,
    private notification: NzNotificationService,
     private districtService: DistrictService,
    private provinceService: ProvinceService
  ) {}

  ngOnInit(): void {  
    this.wardAddForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      administrativeLevel: ['', [Validators.required]],
      note: ['', [Validators.required]],
      provinceCode: [null, [Validators.required]],
      districtCode: [null, [Validators.required]]
    });
    this.loadProvinces();
    this.wardAddForm.get('provinceCode').valueChanges.subscribe(value => {
      this.wardAddForm.get('districtCode').setValue(null);
      this.loadDistricts(value);
    });
  
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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
    if (this.wardAddForm.valid) {
      const formValue = this.wardAddForm.value;
      const WardData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
        provinceCode: Number(formValue.provinceCode)
      };

      this.WardService.create(WardData).subscribe({
        next: response => {
          console.log('Tạo tỉnh thành công:', response);
          this.notification.create('success', 'Thêm thành công', `${response.name}`);
          this.wardAddForm.reset();
          this.visibilityChange.emit({ showAddForm: false, addStatus: true });
        },
        error: (err: ErrorResponse) => {
          this.notification.create('error', 'Lỗi', `${err.error.error.data.message}`);
        },
      });
    } else {
      Object.values(this.wardAddForm.controls).forEach(control => {
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
