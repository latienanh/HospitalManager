import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GetPagingResponse, UserDto, UserHospitalDto } from '@proxy/dtos/response';
import { HospitalService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { catchError, forkJoin, Subject } from 'rxjs';
import { ErrorResponse } from 'src/app/core/models/error-response.model';
import { VisibleAdd } from 'src/app/features/administrative/models/visible';
import { BaseGetPagingRequest } from '../../../../proxy/dtos/common/models';
import { UserHospitalService } from '../../../../proxy/services/user-hospital.service';
import { CreateUpdateUserHospitalDto } from '@proxy/dtos/request/create-update';

@Component({
  selector: 'app-add-user-hospital',
  templateUrl: './add-user-hospital.component.html',
  styleUrls: ['./add-user-hospital.component.scss']
})
export class AddUserHospitalComponent implements OnInit, OnDestroy {
  @Input() id: number;
  @Input() isVisibleAddUserHospital: VisibleAdd;
  @Output() visibilityChange = new EventEmitter<VisibleAdd>();

  private destroy$ = new Subject<void>();

  userHospitalAddForm: FormGroup;
  users: GetPagingResponse<UserDto>={
    data:[],
    totalPage :0
  };

  constructor(
    private fb: FormBuilder,
    private hospitalService: HospitalService,
    private notification: NzNotificationService,
    private userHospitalService: UserHospitalService
  ) {}

  ngOnInit(): void {
    const request: BaseGetPagingRequest = {
      index: 0,
      size: 100
    };
    this.hospitalService.getUserNotInHospitalDapperList(request).subscribe({
      next: (response) => {
        console.log(response)
        this.users = response;
      },
      error: (err) => {
        console.log(err);
      }
    });
    this.userHospitalAddForm = this.fb.group({
      userIdList: [[], [Validators.required]],
      hospitalId: [this.id, [Validators.required]],
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  submitForm(): void {
    if (this.userHospitalAddForm.valid) {
      const formValue = this.userHospitalAddForm.value;
      const requests = formValue.userIdList.map((userId: string) => {
        const data: CreateUpdateUserHospitalDto = {
          userId: userId,
          hospitalId: formValue.hospitalId
        };
        
        return this.userHospitalService.create(data).pipe(
          catchError((err: ErrorResponse) => {
            this.notification.create('error', 'Lỗi', `${err.error.error.data.message}`);
            return []; // Return empty array to continue the request flow
          })
        );
      });
  
      forkJoin(requests).subscribe({
        next: (responses) => {
          this.notification.create('success','Thành công','Tất cả người dùng đã được thêm thành công:');
          this.userHospitalAddForm.reset();
          this.visibilityChange.emit({ showAddForm: false, addStatus: true });
        },
        error: (err) => {
          // Handle the case where forkJoin fails
          console.error('Đã có lỗi xảy ra khi thêm người dùng:', err);
          this.notification.create('error', 'Lỗi', 'Đã có lỗi xảy ra. Vui lòng thử lại!');
        }
      });
    } else {
      Object.values(this.userHospitalAddForm.controls).forEach(control => {
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
