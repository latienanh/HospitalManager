import { PagedResultDto } from '@abp/ng.core';
import { Component } from '@angular/core';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { DistrictDto, GetPagingResponse, ProvinceDto, WardDto } from '@proxy/dtos/response';
import { ProvinceService } from '@proxy/services';
import { VisibleAddProvince, VisibleImportProvince } from '../../models/visibleaddprovince';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-list-province',
  templateUrl: './list-province.component.html',
  styleUrl: './list-province.component.scss',
})
export class ListProvinceComponent {
  isVisibleAddProvince: VisibleAddProvince = {
    addProvinceStatus: false,
    showAddProvinceForm: false,
  };
  isVisibleImportProvince: VisibleImportProvince = {
    importProvinceStatus: false,
    showImportProvinceForm: false,
  };

  provinces: GetPagingResponse<ProvinceDto> = {
    data: [],
    totalPage: 0,
  };

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
    item: ProvinceDto & {
      expand: boolean;
      districts?: PagedResultDto<DistrictDto> & {
        expand: boolean;
        wards?: PagedResultDto<WardDto>;
      };
    }
  ): any {
    return item.id;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProvinces();
  }

  onAdd() {
    this.isVisibleAddProvince.showAddProvinceForm = true;
  }

  handleVisibilityAddChange(event: VisibleAddProvince) {
    this.isVisibleAddProvince.showAddProvinceForm = event.showAddProvinceForm;
    this.isVisibleAddProvince.addProvinceStatus = event.addProvinceStatus;
    console.log(this.isVisibleAddProvince);
    if (this.isVisibleAddProvince.addProvinceStatus) this.loadProvinces();
    else {
      console.log('khong lam gi');
    }
  }
  handleVisibilityImportChange(event: VisibleImportProvince) {
    this.isVisibleImportProvince.showImportProvinceForm = event.showImportProvinceForm;
    this.isVisibleImportProvince.importProvinceStatus = event.importProvinceStatus;
    if (this.isVisibleImportProvince.importProvinceStatus) 
      this.loadProvinces();
    else {
      console.log('khong lam gi');
    }
  }
  onExport() {
    this.isVisibleImportProvince.showImportProvinceForm = true;
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
  onEdit(){
    this.isVisibleAddProvince.showAddProvinceForm = true;
  }
}
