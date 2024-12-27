import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { BaseGetPagingRequest } from '../../dtos/common/models';
import type { CreateUpdatePatientDto } from '../../dtos/request/create-update/models';
import type { GetPagingResponse, HospitalPatientCountDto, PatientDto, ProvincePatientCountDto } from '../../dtos/response/models';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  apiName = 'Default';
  

  create = (input: CreateUpdatePatientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientDto>({
      method: 'POST',
      url: '/api/app/patient',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/patient/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientDto>({
      method: 'GET',
      url: `/api/app/patient/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PatientDto>>({
      method: 'GET',
      url: '/api/app/patient',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPatientDapperList = (request: BaseGetPagingRequest, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GetPagingResponse<PatientDto>>({
      method: 'POST',
      url: '/api/app/patient/get-patient-dapper-list',
      body: request,
    },
    { apiName: this.apiName,...config });
  

  hospitalPatientCountReport = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, HospitalPatientCountDto[]>({
      method: 'POST',
      url: '/api/app/patient/hospital-patient-count-report',
    },
    { apiName: this.apiName,...config });
  

  provincePatientCountReport = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ProvincePatientCountDto[]>({
      method: 'POST',
      url: '/api/app/patient/province-patient-count-report',
    },
    { apiName: this.apiName,...config });
  

  update = (id: number, input: CreateUpdatePatientDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PatientDto>({
      method: 'PUT',
      url: `/api/app/patient/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
