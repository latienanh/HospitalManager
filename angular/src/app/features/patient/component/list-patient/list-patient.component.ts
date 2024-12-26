import { Component, OnInit } from '@angular/core';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingResponse, PatientDto } from '@proxy/dtos/response';
import { PatientService } from '@proxy/patient-manager/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { ErrorResponse } from 'src/app/core/models/error-response.model';
import { VisibleAdd, VisibleUpdate } from 'src/app/features/administrative/models/visible';

@Component({
  selector: 'app-list-patient',

  templateUrl: './list-patient.component.html',
  styleUrl: './list-patient.component.scss',
})
export class ListPatientComponent implements OnInit {
  isVisibleAddPatient: VisibleAdd = {
    addStatus: false,
    showAddForm: false,
  };
  isVisibleUpdatePatient: VisibleUpdate = {
    updateStatus: false,
    showUpdateForm: false,
  };
  patients: GetPagingResponse<PatientDto> = {
    data: [],
    totalPage: 0,
  };
  selectedId: number | null;
  selectedIdAddUser: number | null;
  currentPage: number = 1;
  pageSize: number = 10;

  constructor(
    private patientService: PatientService,
    private notification: NzNotificationService
  ) {}

  ngOnInit() {
    this.loadPatients();
  }

  loadPatients() {
    const query: BaseGetPagingRequest = {
      index: this.currentPage - 1,
      size: this.pageSize,
    };

    this.patientService.getPatientDapperList(query).subscribe({
      next: response => {
        this.patients = response;
        console.log(this.patients);
      },
      error: (err:ErrorResponse)  => {
        console.log(err);
        if(err.status==403)
          this.notification.create('error', 'Lỗi', `${err.error.error.data.message}`);
      },
    });
  }

  trackById(index: number, item: PatientDto): any {
    return item.id;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadPatients();
  }

  onAdd() {
    this.isVisibleAddPatient.showAddForm = true;
  }

  handleVisibilityAddChange(event: VisibleAdd) {
    this.isVisibleAddPatient.showAddForm = event.showAddForm;
    this.isVisibleAddPatient.addStatus = event.addStatus;
    console.log(this.isVisibleAddPatient);
    if (this.isVisibleAddPatient.addStatus) this.loadPatients();
    else {
      console.log('khong lam gi');
    }
  }
  onDelete(id: number) {
    this.patientService.delete(id).subscribe({
      next: response => {
        this.notification.success('Thành công', `Xoá thành công ${response}`);
        this.loadPatients();
      },
      error: error => {
        this.notification.success('Thất bại', `Xoá thất bại ${error}`);
      },
    });
  }
  onEdit(id: number) {
    this.selectedId = id;
    this.isVisibleUpdatePatient.showUpdateForm = true;
  }

  handleVisibilityUpdateChange(event: VisibleUpdate) {
    this.isVisibleUpdatePatient.showUpdateForm = event.showUpdateForm;
    this.isVisibleUpdatePatient.updateStatus = event.updateStatus;
    if (this.isVisibleUpdatePatient.updateStatus) this.loadPatients();
    else {
      console.log('khong lam gi');
    }
    this.selectedId = null;
  }
}
