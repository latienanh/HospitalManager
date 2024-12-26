import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateUpdateUserHospitalDto } from '../dtos/request/create-update/models';
import type { UserHospitalDto } from '../dtos/response/models';

@Injectable({
  providedIn: 'root',
})
export class UserHospitalService {
  apiName = 'Default';
  

  create = (input: CreateUpdateUserHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserHospitalDto>({
      method: 'POST',
      url: '/api/app/user-hospital',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/user-hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserHospitalDto>({
      method: 'GET',
      url: `/api/app/user-hospital/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getHospitalByUserIdByUserId = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, number>({
      method: 'GET',
      url: `/api/app/user-hospital/hospital-by-user-id/${userId}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<UserHospitalDto>>({
      method: 'GET',
      url: '/api/app/user-hospital',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: number, input: CreateUpdateUserHospitalDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserHospitalDto>({
      method: 'PUT',
      url: `/api/app/user-hospital/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
