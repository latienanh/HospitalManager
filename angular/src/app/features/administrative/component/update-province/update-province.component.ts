import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import {
  NonNullableFormBuilder,
  Validators
} from '@angular/forms';
import { ProvinceService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Subject, takeUntil } from 'rxjs';
import { ErrorResponse } from '../../../../core/models/error-response.model';
import { VisibleUpdate } from '../../models/visible';

@Component({
  selector: 'app-update-province',
  templateUrl: './update-province.component.html',
  styleUrl: './update-province.component.scss'
})
export class UpdateProvinceComponent implements OnInit,OnDestroy {
  @Input() id:number;
  @Input() isVisibleUpdateProvince: VisibleUpdate ;
  @Output() visibilityChange = new EventEmitter<VisibleUpdate>();
  handleCancel() {
    this.visibilityChange.emit({ updateStatus: false, showUpdateForm: false });
  }
  private destroy$ = new Subject<void>();

  provinceUpdateForm = this.fb.group({
    code: this.fb.control('', [Validators.required]),
    name: this.fb.control('', [Validators.required]),
    administrativeLevel: this.fb.control('', [Validators.required]),
    note: this.fb.control('', [Validators.required]),
  });

  constructor(
    private fb: NonNullableFormBuilder,
    private provinceService: ProvinceService,
    private notification: NzNotificationService
  ) {
   
  }
  ngOnInit(): void {
    this.provinceService.get(this.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.provinceUpdateForm.patchValue({
            code: response.code.toString(),
            name: response.name,
            administrativeLevel: response.administrativeLevel,
            note: response.note
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

  submitForm(): void {
    if (this.provinceUpdateForm.valid) {
      const formValue = this.provinceUpdateForm.value;

      const provinceData = {
        ...formValue,
        code: Number(formValue.code) // Chuyển đổi code từ string thành number
      };

      this.provinceService.update(this.id,provinceData).subscribe({
        next: response => {
          console.log('Cập nhật tỉnh thành công:', response);
          this.notification.create(
            'success',
            'Cập nhật thành công',
            `${response.name}`
          );
          this.provinceUpdateForm.reset();
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
      Object.values(this.provinceUpdateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
