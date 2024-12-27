import { Component, OnInit } from '@angular/core';
import { VisibleAdd, VisibleImport, VisibleUpdate } from '../../models/visible';
import { DistrictDto, GetPagingResponse, ProvinceDto } from '@proxy/dtos/response';
import { DistrictService, ProvinceService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingDistrictRequest } from '@proxy/dtos/request/get-paging';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-list-district',
  templateUrl: './list-district.component.html',
  styleUrl: './list-district.component.scss'
})
export class ListDistrictComponent implements OnInit{
  isVisibleAddDistrict: VisibleAdd = {
    addStatus: false,
    showAddForm: false,
  };
  isVisibleImportDistrict: VisibleImport = {
    importStatus: false,
    showImportForm: false,
  };
  isVisibleUpdateDistrict: VisibleUpdate = {
    updateStatus: false,
    showUpdateForm: false,
  };
  districts: GetPagingResponse<DistrictDto> = {
    data: [],
    totalPage: 0,
  };
  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };
  selectedId:number|null
  currentPage: number = 1;
  pageSize: number = 10;
  form: FormGroup;
  constructor(private districtService: DistrictService,private provinceService :ProvinceService,private notification:NzNotificationService) {}

  ngOnInit() {
    this.form = new FormGroup({
      selectedValue: new FormControl(null)
    });
    
    this.loadProvinces();
    this.loadDistricts();
    this.form.get('selectedValue').valueChanges.subscribe(value => {
      this.currentPage = 1
      this.loadDistricts(value);
    });
  }

  loadDistricts(provincecode?:number) {
    const query: GetPagingDistrictRequest = {
      index: this.currentPage - 1,
      size: this.pageSize,
      provinceCode:provincecode,
    };

    this.districtService.getDistrictDapperList(query).subscribe({
      next: response => {
       
        this.districts = response;
        console.log(this.districts)
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
        console.log(this.provinces)
      },
      error: err => {
        console.log(err);
      },
    });
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadDistricts(this.form.value.selectedValue);
    
  }

  onAdd() {
    this.isVisibleAddDistrict.showAddForm = true;
  }

  handleVisibilityAddChange(event: VisibleAdd) {
    this.isVisibleAddDistrict.showAddForm = event.showAddForm;
    this.isVisibleAddDistrict.addStatus = event.addStatus;
    console.log(this.isVisibleAddDistrict);
    if (this.isVisibleAddDistrict.addStatus) this.loadDistricts(this.form.value.selectedValue);
    else {
      console.log('khong lam gi');
    }
  }
  handleVisibilityImportChange(event: VisibleImport) {
    this.isVisibleImportDistrict.showImportForm = event.showImportForm;
    this.isVisibleImportDistrict.importStatus = event.importStatus;
    if (this.isVisibleImportDistrict.importStatus) 
      this.loadDistricts(this.form.value.selectedValue);
    else {
      console.log('khong lam gi');
    }
  }
  trackById(
      index: number,
      item: DistrictDto
    ): any {
      return item.id;
    }
  onExport() {
    this.isVisibleImportDistrict.showImportForm = true;
  }
  onDelete(id:number){
    this.districtService.delete(id).subscribe(
      {
        next:(response)=>{
          console.log(response)
          this.notification.success('Thành công', `Xoá thành công `);
          this.loadDistricts(this.form.value.selectedValue);
        },
        error:(error)=>{
          this.notification.success('Thất bại', `Xoá thất bại ${error}`);
        }
      }
    )
  }
  onEdit(id:number){
    this.selectedId = id;
    this.isVisibleUpdateDistrict.showUpdateForm = true;
  }
  handleVisibilityUpdateChange(event: VisibleUpdate){
    this.isVisibleUpdateDistrict.showUpdateForm = event.showUpdateForm;
    this.isVisibleUpdateDistrict.updateStatus = event.updateStatus;
    if (this.isVisibleUpdateDistrict.updateStatus) 
      this.loadDistricts(this.form.value.selectedValue);
    else {
      console.log('khong lam gi');
    }
    this.selectedId = null
  }

}
