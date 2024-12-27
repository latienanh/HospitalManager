import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListDistrictComponent } from './component/list-district/list-district.component';
import { ListProvinceComponent } from './component/list-province/list-province.component';
import { ListWardComponent } from './component/list-ward/list-ward.component';
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  {
    path: 'provinces',
    component: ListProvinceComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'Province_Manager', // policy key for your component
    },
  },
  {
    path: 'districts',
    component: ListDistrictComponent,

    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'District_Manager', // policy key for your component
    },
  },
  {
    path: 'wards',
    component: ListWardComponent,
    canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'Ward_Manager', // policy key for your component
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdministrativeRoutingModule {}
