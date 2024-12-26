import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { FormBuilder, NonNullableFormBuilder, Validators } from '@angular/forms';
import { ProvinceService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Subject } from 'rxjs';
import { ErrorResponse } from '../../../../core/models/error-response.model';
import { VisibleAdd } from '../../models/visible';
@Component({
  selector: 'app-add-province',
  templateUrl: './add-province.component.html',
  styleUrl: './add-province.component.scss',
})
export class AddProvinceComponent implements OnDestroy {
  @Input() isVisibleAddProvince: VisibleAdd;
  @Output() visibilityChange = new EventEmitter<VisibleAdd>();
  handleCancel() {
    this.visibilityChange.emit({ addStatus: false, showAddForm: false });
  }
  private destroy$ = new Subject<void>();

  provinceAddForm = this.fb.group({
    code: this.fb.control('', [Validators.required]),
    name: this.fb.control('', [Validators.required]),
    administrativeLevel: this.fb.control('', [Validators.required]),
    note: this.fb.control('', [Validators.required]),
  });

  constructor(
    private fb: FormBuilder,
    private provinceService: ProvinceService,
    private notification: NzNotificationService
  ) {}

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  submitForm(): void {
    if (this.provinceAddForm.valid) {
      const formValue = this.provinceAddForm.value;

      const provinceData = {
        ...formValue,
        code: Number(formValue.code), // Chuyển đổi code từ string thành number
      };

      this.provinceService.create(provinceData).subscribe({
        next: response => {
          console.log('Tạo tỉnh thành công:', response);
          this.notification.create('success', 'Thêm thành công', `${response.name}`);
          this.provinceAddForm.reset();
          this.visibilityChange.emit({ showAddForm: false, addStatus: true });
        },
        error: (err: ErrorResponse) => {
          console.log(err);
          this.notification.create('error', 'Lỗi', `${err.error.error.data.message}`);
        },
      });
    } else {
      Object.values(this.provinceAddForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
