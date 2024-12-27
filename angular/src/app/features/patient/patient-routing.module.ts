import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListPatientComponent } from './component/list-patient/list-patient.component';
import { permissionGuard } from '@abp/ng.core';

const routes: Routes = [{ path: '', component: ListPatientComponent ,
  canActivate: [permissionGuard],
    data: {
      requiredPolicy: 'Patient_Manager', // policy key for your component
    },}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule { }
