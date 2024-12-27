import { Component, OnInit } from '@angular/core';
import { VisibleAdd, VisibleImport, VisibleUpdate } from '../../models/visible';
import { DistrictDto, GetPagingResponse, ProvinceDto, WardDto } from '@proxy/dtos/response';
import { FormControl, FormGroup } from '@angular/forms';
import { DistrictService, WardService } from '@proxy/services';
import { ProvinceService } from '../../../../proxy/services/province.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingDistrictRequest, GetPagingWardRequest } from '@proxy/dtos/request/get-paging';

@Component({
  selector: 'app-list-ward',
  templateUrl: './list-ward.component.html',
  styleUrl: './list-ward.component.scss',
})
export class ListWardComponent implements OnInit {
  isVisibleAddWard: VisibleAdd = {
    addStatus: false,
    showAddForm: false,
  };
  isVisibleImportWard: VisibleImport = {
    importStatus: false,
    showImportForm: false,
  };
  isVisibleUpdateWard: VisibleUpdate = {
    updateStatus: false,
    showUpdateForm: false,
  };
  wards: GetPagingResponse<WardDto> = {
    data: [],
    totalPage: 0,
  };
  districts: GetPagingResponse<DistrictDto> = {
    data: [],
    totalPage: 0,
  };
  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };
  selectedId: number | null;
  currentPage: number = 1;
  pageSize: number = 10;
  form: FormGroup;
  constructor(
    private districtService: DistrictService,
    private wardService: WardService,
    private provinceService: ProvinceService,
    private notification: NzNotificationService
  ) {}

  ngOnInit() {
    this.form = new FormGroup({
      selectedValueProvince: new FormControl(null),
      selectedValueDistrict: new FormControl(null),
    });

    this.loadProvinces();
    this.loadWards();
    this.form.get('selectedValueProvince').valueChanges.subscribe(value => {
      this.form.get('selectedValueDistrict').setValue(null);
      if(!value)
      {
        this.districts.data = []
      }
        
      this.loadDistricts(value);
      this.loadWards(value);
    });
    this.form.get('selectedValueDistrict').valueChanges.subscribe(value => {
      this.currentPage =1 
      this.loadWards(this.form.value.selectedValueProvince,value);
    });
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
        console.log(this.provinces);
      },
      error: err => {
        console.log(err);
      },
    });
  }
  loadWards(provinceCode?:number,districtCode?:number) {
    console.log(provinceCode,districtCode)
    const query: GetPagingWardRequest = {
      index: this.currentPage-1,
      size: this.pageSize,
      provinceCode:provinceCode,
      districtCode:districtCode
    };
    this.wardService.getWardDapperList(query).subscribe({
      next: response => {
        this.wards = response;
        console.log(this.wards)
      },
      error: err => {
        console.log(err);
      },
    });
  }
  onPageChange(page: number): void {
    this.currentPage = page;
    console.log(this.currentPage)
    this.loadWards(this.form.value.selectedValueProvince,this.form.value.selectedValueDistrict);
  }

  onAdd() {
    this.isVisibleAddWard.showAddForm = true;
  }

  handleVisibilityAddChange(event: VisibleAdd) {
    this.isVisibleAddWard.showAddForm = event.showAddForm;
    this.isVisibleAddWard.addStatus = event.addStatus;
    console.log(this.isVisibleAddWard);
    if (this.isVisibleAddWard.addStatus) this.loadWards(this.form.value.selectedValueProvince,this.form.value.selectedValueDistrict);
    else {
      console.log('khong lam gi');
    }
  }
  handleVisibilityImportChange(event: VisibleImport) {
    this.isVisibleImportWard.showImportForm = event.showImportForm;
    this.isVisibleImportWard.importStatus = event.importStatus;
    if (this.isVisibleImportWard.importStatus) this.loadWards(this.form.value.selectedValueProvince,this.form.value.selectedValueDistrict);
    else {
      console.log('khong lam gi');
    }
  }
  trackById(index: number, item: DistrictDto): any {
    return item.id;
  }
  onExport() {
    this.isVisibleImportWard.showImportForm = true;
  }
  onDelete(id: number) {
    this.wardService.delete(id).subscribe({
      next: response => {
        this.notification.success('Thành công', `Xoá thành công ${response}`);
        this.loadWards(this.form.value.selectedValueProvince,this.form.value.selectedValueDistrict);
      },
      error: error => {
        this.notification.success('Thất bại', `Xoá thất bại ${error}`);
      },
    });
  }
  onEdit(id: number) {
    this.selectedId = id;
    this.isVisibleUpdateWard.showUpdateForm = true;
  }
  handleVisibilityUpdateChange(event: VisibleUpdate) {
    this.isVisibleUpdateWard.showUpdateForm = event.showUpdateForm;
    this.isVisibleUpdateWard.updateStatus = event.updateStatus;
    if (this.isVisibleUpdateWard.updateStatus) this.loadWards(this.form.value.selectedValueProvince,this.form.value.selectedValueDistrict);
    else {
      console.log('khong lam gi');
    }
    this.selectedId = null;
  }
}
