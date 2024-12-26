import { PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { DistrictDto, GetPagingResponse, ProvinceDto, WardDto } from '@proxy/dtos/response';
import { ProvinceService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { VisibleAdd, VisibleImport, VisibleUpdate } from '../../models/visible';

@Component({
  selector: 'app-list-province',
  templateUrl: './list-province.component.html',
  styleUrl: './list-province.component.scss',
})
export class ListProvinceComponent implements OnInit {
  isVisibleAddProvince: VisibleAdd = {
    addStatus: false,
    showAddForm: false,
  };
  isVisibleImportProvince: VisibleImport = {
    importStatus: false,
    showImportForm: false,
  };
  isVisibleUpdateProvince: VisibleUpdate = {
    updateStatus: false,
    showUpdateForm: false,
  };
  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };
  selectedId:number|null
  currentPage: number = 1;
  pageSize: number = 10;

  constructor(private provinceService: ProvinceService,private notification:NzNotificationService) {}

  ngOnInit() {
    this.loadProvinces();
  }

  loadProvinces() {
    const query: BaseGetPagingRequest = {
      index: this.currentPage - 1,
      size: this.pageSize,
    };

    this.provinceService.getProvinceDapperList(query).subscribe({
      next: response => {
       
        this.provinces = response;
        console.log(this.provinces)
      },
      error: err => {
        console.log(err);
      },
    });
  }

  trackById(
    index: number,
    item: ProvinceDto
  ): any {
    return item.id;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProvinces();
  }

  onAdd() {
    this.isVisibleAddProvince.showAddForm = true;
  }

  handleVisibilityAddChange(event: VisibleAdd) {
    this.isVisibleAddProvince.showAddForm = event.showAddForm;
    this.isVisibleAddProvince.addStatus = event.addStatus;
    console.log(this.isVisibleAddProvince);
    if (this.isVisibleAddProvince.addStatus) this.loadProvinces();
    else {
      console.log('khong lam gi');
    }
  }
  handleVisibilityImportChange(event: VisibleImport) {
    this.isVisibleImportProvince.showImportForm = event.showImportForm;
    this.isVisibleImportProvince.importStatus = event.importStatus;
    if (this.isVisibleImportProvince.importStatus) 
      this.loadProvinces();
    else {
      console.log('khong lam gi');
    }
  }
  onExport() {
    this.isVisibleImportProvince.showImportForm = true;
  }
  onDelete(id:number){
    this.provinceService.delete(id).subscribe(
      {
        next:(response)=>{
          this.notification.success('Thành công', `Xoá thành công ${response}`);
          this.loadProvinces();
        },
        error:(error)=>{
          this.notification.success('Thất bại', `Xoá thất bại ${error}`);
        }
      }
    )
  }
  onEdit(id:number){
    this.selectedId = id;
    this.isVisibleUpdateProvince.showUpdateForm = true;
  }
  handleVisibilityUpdateChange(event: VisibleUpdate){
    this.isVisibleUpdateProvince.showUpdateForm = event.showUpdateForm;
    this.isVisibleUpdateProvince.updateStatus = event.updateStatus;
    if (this.isVisibleUpdateProvince.updateStatus) 
      this.loadProvinces();
    else {
      console.log('khong lam gi');
    }
    this.selectedId = null
  }
}
