import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListHospitalComponent } from './component/list-hospital/list-hospital.component';
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [{ path: '', component: ListHospitalComponent,canActivate: [permissionGuard],
  data: {
    requiredPolicy: ['Hospital_Manager'], // policy key for your component
  }, }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HospitalRoutingModule { }
