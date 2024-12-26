import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateDistrictDto } from '../dtos/request/create-update/models';
import type { GetPagingDistrictRequest } from '../dtos/request/get-paging/models';
import type { DistrictDto, GetPagingResponse } from '../dtos/response/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';
import type { FileStreamResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class DistrictService {
  apiName = 'Default';
  

  create = (input: CreateUpdateDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>({
      method: 'POST',
      url: '/api/app/district',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/district/${id}`,
    },
    { apiName: this.apiName,...config });
  

  exportExcel = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileStreamResult>({
      method: 'POST',
      url: '/api/app/district/export-excel',
    },
    { apiName: this.apiName,...config });
  

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>({
      method: 'GET',
      url: `/api/app/district/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getDistrictDapperList = (request: GetPagingDistrictRequest, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPagingResponse<DistrictDto>>({
      method: 'POST',
      url: '/api/app/district/get-district-dapper-list',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DistrictDto>>({
      method: 'GET',
      url: '/api/app/district',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  importExcelByFileAndIsUpdate = (file: any, isUpdate: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/district/import-excel',
      params: { isUpdate },
      body: file,
    },
    { apiName: this.apiName,...config });
  

  update = (id: number, input: CreateUpdateDistrictDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistrictDto>({
      method: 'PUT',
      url: `/api/app/district/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
