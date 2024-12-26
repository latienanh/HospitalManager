import { Component, OnInit } from '@angular/core';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingResponse, HospitalDto } from '@proxy/dtos/response';
import { HospitalService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { VisibleAdd, VisibleImport, VisibleUpdate } from 'src/app/features/administrative/models/visible';

@Component({
  selector: 'app-list-hospital',
  templateUrl: './list-hospital.component.html',
  styleUrl: './list-hospital.component.scss'
})
export class ListHospitalComponent implements OnInit{
  isVisibleAddHospital: VisibleAdd = {
    addStatus: false,
    showAddForm: false,
  };
  isVisibleAddUserHospital: VisibleAdd = {
    addStatus: false,
    showAddForm: false,
  };
  isVisibleUpdateHospital: VisibleUpdate = {
    updateStatus: false,
    showUpdateForm: false,
  };
  hospitals: GetPagingResponse<HospitalDto> = {
    data: [],
    totalPage: 0,
  };
  selectedId:number|null
  selectedIdAddUser:number|null
  currentPage: number = 1;
  pageSize: number = 10;

  constructor(private hospitalService: HospitalService,private notification:NzNotificationService) {}

  ngOnInit() {
    this.loadHospitals();
  }

  loadHospitals() {
    const query: BaseGetPagingRequest = {
      index: this.currentPage - 1,
      size: this.pageSize,
    };

    this.hospitalService.getHospitalDapperList(query).subscribe({
      next: response => {
       
        this.hospitals = response;
        console.log(this.hospitals)
      },
      error: err => {
        console.log(err);
      },
    });
  }

  trackById(
    index: number,
    item: HospitalDto
  ): any {
    return item.id;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadHospitals();
  }

  onAdd() {
    this.isVisibleAddHospital.showAddForm = true;
  }

  handleVisibilityAddChange(event: VisibleAdd) {
    this.isVisibleAddHospital.showAddForm = event.showAddForm;
    this.isVisibleAddHospital.addStatus = event.addStatus;
    console.log(this.isVisibleAddHospital);
    if (this.isVisibleAddHospital.addStatus) this.loadHospitals();
    else {
      console.log('khong lam gi');
    }
  }
  handleVisibilityAddUserChange(event: VisibleAdd) {
    this.isVisibleAddUserHospital.showAddForm = event.showAddForm;
    this.isVisibleAddUserHospital.addStatus = event.addStatus;
    if (this.isVisibleAddUserHospital.addStatus) 
      this.loadHospitals();
    else {
      console.log('khong lam gi');
    }
  }
  onExport() {
    this.isVisibleAddUserHospital.addStatus = true;
  }
  onDelete(id:number){
    this.hospitalService.delete(id).subscribe(
      {
        next:(response)=>{
          this.notification.success('Thành công', `Xoá thành công ${response}`);
          this.loadHospitals();
        },
        error:(error)=>{
          this.notification.success('Thất bại', `Xoá thất bại ${error}`);
        }
      }
    )
  }
  onEdit(id:number){
    this.selectedId = id;
    this.isVisibleUpdateHospital.showUpdateForm = true;
  }
  onAddUser(id:number){
    this.selectedIdAddUser = id;
    this.isVisibleAddUserHospital.showAddForm = true;
  }
  handleVisibilityUpdateChange(event: VisibleUpdate){
    this.isVisibleUpdateHospital.showUpdateForm = event.showUpdateForm;
    this.isVisibleUpdateHospital.updateStatus = event.updateStatus;
    if (this.isVisibleUpdateHospital.updateStatus) 
      this.loadHospitals();
    else {
      console.log('khong lam gi');
    }
    this.selectedId = null
  }
}
