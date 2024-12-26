import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientRoutingModule } from './patient-routing.module';
import { ListPatientComponent } from './component/list-patient/list-patient.component';
import { AddPatientComponent } from './component/add-patient/add-patient.component';
import { UpdatePatientComponent } from './component/update-patient/update-patient.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';


@NgModule({
  declarations: [ListPatientComponent,AddPatientComponent,UpdatePatientComponent],
  imports: [
    CommonModule,
    PatientRoutingModule,
    SharedModule,
    NzTableModule,
    NzButtonModule,
    NzModalModule
  ]
})
export class PatientModule { }
