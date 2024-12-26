import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { VisibleUpdate } from '../../models/visible';
import { Subject, takeUntil } from 'rxjs';
import { NonNullableFormBuilder, Validators } from '@angular/forms';
import { DistrictService, ProvinceService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { GetPagingResponse, ProvinceDto } from '@proxy/dtos/response';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { ErrorResponse } from 'src/app/core/models/error-response.model';

@Component({
  selector: 'app-update-district',
  templateUrl: './update-district.component.html',
  styleUrl: './update-district.component.scss'
})
export class UpdateDistrictComponent implements OnInit,OnDestroy {
  @Input() id:number;
  @Input() isVisibleUpdateDistrict: VisibleUpdate ;
  @Output() visibilityChange = new EventEmitter<VisibleUpdate>();
  handleCancel() {
    this.visibilityChange.emit({ updateStatus: false, showUpdateForm: false });
  }
  private destroy$ = new Subject<void>();
  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };
  districtUpdateForm = this.fb.group({
    code: this.fb.control('', [Validators.required]),
    name: this.fb.control('', [Validators.required]),
    administrativeLevel: this.fb.control('', [Validators.required]),
    note: this.fb.control('', [Validators.required]),
    provinceCode: this.fb.control<string | number | null>(null, [Validators.required])
  });

  constructor(
    private fb: NonNullableFormBuilder,
    private districtService: DistrictService,
    private notification: NzNotificationService,
     private provinceService: ProvinceService
  ) {
   
  }
  ngOnInit(): void {
    this.loadProvinces();
    this.districtService.get(this.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.districtUpdateForm.patchValue({
            code: response.code.toString(),
            name: response.name,
            administrativeLevel: response.administrativeLevel,
            note: response.note,
            provinceCode: response.provinceCode
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
    if (this.districtUpdateForm.valid) {
      const formValue = this.districtUpdateForm.value;

      const districtData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
        provinceCode: Number(formValue.provinceCode)
      };

      this.districtService.update(this.id,districtData).subscribe({
        next: response => {
          console.log('Cập nhật tỉnh thành công:', response);
          this.notification.create(
            'success',
            'Cập nhật thành công',
            `${response.name}`
          );
          this.districtUpdateForm.reset();
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
      Object.values(this.districtUpdateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
