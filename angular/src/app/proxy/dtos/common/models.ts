import type { AuditedEntityDto } from '@abp/ng.core';

export interface BaseGetPagingRequest {
  index: number;
  size: number;
}

export interface BaseDto<T> extends AuditedEntityDto<number> {
  code: T;
  name?: string;
}

export interface CommonAdministrativeDto extends BaseDto<number> {
  administrativeLevel?: string;
  note?: string;
}
