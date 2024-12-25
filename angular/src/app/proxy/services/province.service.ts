import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseGetPagingRequest } from '../dtos/common/models';
import type { CreateUpdateProvinceDto } from '../dtos/request/create-update/models';
import type { GetPagingResponse, ProvinceDto } from '../dtos/response/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';
import type { FileStreamResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class ProvinceService {
  apiName = 'Default';
  

  create = (input: CreateUpdateProvinceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>({
      method: 'POST',
      url: '/api/app/province',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/province/${id}`,
    },
    { apiName: this.apiName,...config });
  

  exportExcel = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileStreamResult>({
      method: 'POST',
      url: '/api/app/province/export-excel',
    },
    { apiName: this.apiName,...config });
  

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>({
      method: 'GET',
      url: `/api/app/province/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ProvinceDto>>({
      method: 'GET',
      url: '/api/app/province',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getProvinceDapperList = (request: BaseGetPagingRequest, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPagingResponse<ProvinceDto>>({
      method: 'POST',
      url: '/api/app/province/get-province-dapper-list',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  importExcelByFileAndIsUpdate = (file: any, isUpdate: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/province/import-excel',
      params: { isUpdate },
      body: file,
    },
    { apiName: this.apiName,...config });
  

  update = (id: number, input: CreateUpdateProvinceDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvinceDto>({
      method: 'PUT',
      url: `/api/app/province/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
