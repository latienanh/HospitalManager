import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListDistrictComponent } from './component/list-district/list-district.component';
import { ListProvinceComponent } from './component/list-province/list-province.component';
import { ListWardComponent } from './component/list-ward/list-ward.component';

const routes: Routes = [
  { path: 'provinces', component: ListProvinceComponent },
  { path: 'districts', component: ListDistrictComponent },
  { path: 'wards', component: ListWardComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministrativeRoutingModule { }
