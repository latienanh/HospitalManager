import type { BaseGetPagingRequest } from '../../common/models';

export interface GetPagingDistrictRequest extends BaseGetPagingRequest {
  provinceCode?: number;
}

export interface GetPagingWardRequest extends BaseGetPagingRequest {
  districtCode?: number;
  provinceCode?: number;
}
