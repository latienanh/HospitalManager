
export interface BaseCreateUpdate {
  code: number;
  name?: string;
}

export interface CreateUpdateAdministrative extends BaseCreateUpdate {
  administrativeLevel?: string;
  note?: string;
}
