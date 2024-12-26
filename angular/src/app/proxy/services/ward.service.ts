import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateWardDto } from '../dtos/request/create-update/models';
import type { GetPagingWardRequest } from '../dtos/request/get-paging/models';
import type { GetPagingResponse, WardDto } from '../dtos/response/models';
import type { IFormFile } from '../microsoft/asp-net-core/http/models';
import type { FileStreamResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class WardService {
  apiName = 'Default';
  

  create = (input: CreateUpdateWardDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WardDto>({
      method: 'POST',
      url: '/api/app/ward',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/ward/${id}`,
    },
    { apiName: this.apiName,...config });
  

  exportExcel = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileStreamResult>({
      method: 'POST',
      url: '/api/app/ward/export-excel',
    },
    { apiName: this.apiName,...config });
  

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WardDto>({
      method: 'GET',
      url: `/api/app/ward/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<WardDto>>({
      method: 'GET',
      url: '/api/app/ward',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getWardDapperList = (request: GetPagingWardRequest, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPagingResponse<WardDto>>({
      method: 'POST',
      url: '/api/app/ward/get-ward-dapper-list',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  importExcelByFileAndIsUpdate = (file: any, isUpdate: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/api/app/ward/import-excel',
      params: { isUpdate },
      body: file,
    },
    { apiName: this.apiName,...config });
  

  update = (id: number, input: CreateUpdateWardDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WardDto>({
      method: 'PUT',
      url: `/api/app/ward/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
