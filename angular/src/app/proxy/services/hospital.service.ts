import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseGetPagingRequest } from '../dtos/common/models';
import type { CreateUpdateHospitalDto } from '../dtos/request/create-update/models';
import type { GetPagingResponse, HospitalDto, UserDto } from '../dtos/response/models';

@Injectable({
  providedIn: 'root',
})
export class HospitalService {
  apiName = 'Default';
  

  create = (input: CreateUpdateHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'POST',
      url: '/api/app/hospital',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'GET',
      url: `/api/app/hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getHospitalDapperList = (request: BaseGetPagingRequest, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPagingResponse<HospitalDto>>({
      method: 'POST',
      url: '/api/app/hospital/get-hospital-dapper-list',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<HospitalDto>>({
      method: 'GET',
      url: '/api/app/hospital',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getUserByHospitalIdByHospitalId = (HospitalId: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserDto[]>({
      method: 'GET',
      url: `/api/app/hospital/user-by-hospital-id/${HospitalId}`,
    },
    { apiName: this.apiName,...config });
  

  getUserNotInHospitalDapperList = (request: BaseGetPagingRequest, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPagingResponse<UserDto>>({
      method: 'POST',
      url: '/api/app/hospital/get-user-not-in-hospital-dapper-list',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  update = (id: number, input: CreateUpdateHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalDto>({
      method: 'PUT',
      url: `/api/app/hospital/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
