import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingResponse, ProvinceDto } from '@proxy/dtos/response';
import { DistrictService, ProvinceService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Subject } from 'rxjs';
import { ErrorResponse } from 'src/app/core/models/error-response.model';
import { VisibleAdd } from '../../models/visible';

@Component({
  selector: 'app-add-district',
  templateUrl: './add-district.component.html',
  styleUrl: './add-district.component.scss'
})
export class AddDistrictComponent implements OnInit, OnDestroy {
  @Input() isVisibleAddDistrict: VisibleAdd;
  @Output() visibilityChange = new EventEmitter<VisibleAdd>();

  private destroy$ = new Subject<void>();
  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };

  DistrictAddForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private DistrictService: DistrictService,
    private notification: NzNotificationService,
    private provinceService: ProvinceService
  ) {}

  ngOnInit(): void {
    this.loadProvinces();
    this.DistrictAddForm = this.fb.group({
      code: ['', [Validators.required]],
      name: ['', [Validators.required]],
      administrativeLevel: ['', [Validators.required]],
      note: ['', [Validators.required]],
      provinceCode: [null, [Validators.required]]
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
    if (this.DistrictAddForm.valid) {
      const formValue = this.DistrictAddForm.value;
      const DistrictData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
        provinceCode: Number(formValue.provinceCode)
      };

      this.DistrictService.create(DistrictData).subscribe({
        next: response => {
          console.log('Tạo tỉnh thành công:', response);
          this.notification.create('success', 'Thêm thành công', `${response.name}`);
          this.DistrictAddForm.reset();
          this.visibilityChange.emit({ showAddForm: false, addStatus: true });
        },
        error: (err: ErrorResponse) => {
          this.notification.create('error', 'Lỗi', `${err.error.error.data.message}`);
        },
      });
    } else {
      Object.values(this.DistrictAddForm.controls).forEach(control => {
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
