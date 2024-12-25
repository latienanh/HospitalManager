import type { BaseDto, CommonAdministrativeDto } from '../common/models';
import type { AuditedEntityDto } from '@abp/ng.core';

export interface GetPagingResponse<T> {
  data: T[];
  totalPage: number;
}

export interface PatientDto extends BaseDto<number> {
  provinceCode: number;
  districtCode: number;
  wardCode: number;
  address?: string;
}

export interface DistrictDto extends CommonAdministrativeDto {
  provinceCode: number;
}

export interface HospitalDto extends BaseDto<number> {
  provinceCode: number;
  districtCode: number;
  wardCode: number;
  address?: string;
}

export interface ProvinceDto extends CommonAdministrativeDto {
}

export interface UserHospitalDto extends AuditedEntityDto<number> {
  userId?: string;
  hospitalId: number;
}

export interface WardDto extends CommonAdministrativeDto {
}
