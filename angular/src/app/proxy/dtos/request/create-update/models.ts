import type { BaseCreateUpdate, CreateUpdateAdministrative } from './common/models';

export interface CreateUpdatePatientDto extends BaseCreateUpdate {
  provinceCode: number;
  districtCode: number;
  wardCode: number;
  address?: string;
  hospitalId?: number;
}

export interface CreateUpdateDistrictDto extends CreateUpdateAdministrative {
  provinceCode: number;
}

export interface CreateUpdateHospitalDto extends BaseCreateUpdate {
  provinceCode: number;
  districtCode: number;
  wardCode: number;
  address?: string;
}

export interface CreateUpdateProvinceDto extends CreateUpdateAdministrative {
}

export interface CreateUpdateUserHospitalDto {
  userId?: string;
  hospitalId: number;
}

export interface CreateUpdateWardDto extends CreateUpdateAdministrative {
  districtCode: number;
  provinceCode: number;
}
