import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BaseGetPagingRequest } from '@proxy/dtos/common';
import { GetPagingDistrictRequest, GetPagingWardRequest } from '@proxy/dtos/request/get-paging';
import { DistrictDto, GetPagingResponse, ProvinceDto, WardDto } from '@proxy/dtos/response';
import { ProvinceService, DistrictService, WardService } from '@proxy/services';
import { map } from 'rxjs';
import { VisibleAddProvince, VisibleImportProvince } from '../../models/visibleaddprovince';



@Component({
  selector: 'app-administrative',
  templateUrl: './administrative.component.html',
  styleUrls: ['./administrative.component.scss'],
  providers: [ListService],
})
export class AdministrativeComponent implements OnInit {
  isVisibleAddProvince :VisibleAddProvince = {
    addProvinceStatus : false,
    showAddProvinceForm : false
  };
  isVisibleImportProvince :VisibleImportProvince = {
    importProvinceStatus : false,
    showImportProvinceForm : false
  };

  provinces: PagedResultDto<ProvinceDto> = {
    items: [],
    totalCount: 0,
  };

  currentPage: number = 1;
  pageSize: number = 10;

  constructor(
    
    public readonly list: ListService,
    private provinceService: ProvinceService,
    private districtService: DistrictService,
    private wardService: WardService
  ) {}

  ngOnInit() {
    this.loadProvinces();
  }

  loadProvinces() {
    const query: BaseGetPagingRequest = {
      index: this.currentPage - 1,
      size: this.pageSize,
    };

    this.list
      .hookToQuery(() => this.provinceStreamCreator(query))
      .subscribe(response => {
        this.provinces = response;
        console.log(this.provinces);
      });
  }

  provinceStreamCreator(query: BaseGetPagingRequest) {
    return this.provinceService.getProvinceDapperList(query).pipe(
      map((response: GetPagingResponse<ProvinceDto>) => ({
        items: response.data.map(x => ({ ...x, expand: false })),
        totalCount: response.totalPage,
      }))
    );
  }

  trackById(index: number, item: ProvinceDto &{
    expand: boolean;
    districts?: PagedResultDto<DistrictDto> & {
      expand: boolean;
      wards?: PagedResultDto<WardDto>;
    }}): any {
    return item.id;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadProvinces();
  }

  onExpandChangeProvince(expanded: boolean, data: ProvinceDto &{
    expand: boolean;
    districts?: PagedResultDto<DistrictDto> & {
      expand: boolean;
      wards?: PagedResultDto<WardDto>;
    }}) {
    if (expanded && !data.districts) {
      this.loadDistricts(data);
    }
  }

  onExpandChangeDistrict(expanded: boolean, districtData: DistrictDto & {
    expand: boolean;
    wards?: PagedResultDto<WardDto>;
  }) {
    if (expanded && !districtData.wards) {
      this.loadWards(districtData);
    }
  }

  loadDistricts(data:ProvinceDto &{
    expand: boolean;
    districts?: PagedResultDto<DistrictDto> & {
      expand: boolean;
      wards?: PagedResultDto<WardDto>;
    }}) {
    const request: GetPagingDistrictRequest = {
      index: 0,
      size: 10,
      provinceCode: data.code,
    };

    this.districtService.getDistrictDapperList(request).subscribe(
      (response: GetPagingResponse<DistrictDto>) => {
        data.districts = {
          expand: false,
          items: response.data.map(x => ({ ...x, expand: false })),
          totalCount: response.totalPage,
        };
      }
    );
  }

  loadWards(districtData: DistrictDto & { expand: boolean ,wards?: PagedResultDto<WardDto>; }) {
    const request: GetPagingWardRequest = {
      index: 0,
      size: 10,
      districtCode: districtData.code,
      provinceCode: districtData.provinceCode,
    };

    this.wardService.getWardDapperList(request).subscribe(
      (response: GetPagingResponse<WardDto>) => {
        districtData.wards = {
          items: response.data,
          totalCount: response.totalPage,
        };
      }
    );
  }

  onAdd() {
    this.isVisibleAddProvince.showAddProvinceForm = true;
  }

  handleVisibilityChange(event: VisibleAddProvince) {
    this.isVisibleAddProvince.showAddProvinceForm = event.showAddProvinceForm;
    this.isVisibleAddProvince.addProvinceStatus = event.addProvinceStatus;
    console.log(this.isVisibleAddProvince)
    if(this.isVisibleAddProvince.addProvinceStatus)
    this.loadProvinces();
    else{
      console.log("khong lam gi")
    }
  }

  onExport() {
    this.isVisibleImportProvince.showImportProvinceForm = true;
  }
}
