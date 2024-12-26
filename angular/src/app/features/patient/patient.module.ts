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
import { NzInputModule } from 'ng-zorro-antd/input';
import { ReactiveFormsModule } from '@angular/forms';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { HospitalRoutingModule } from '../hospital/hospital-routing.module';

@NgModule({
  declarations: [ListPatientComponent, AddPatientComponent, UpdatePatientComponent],
  imports: [
    CommonModule,
    PatientRoutingModule,
    SharedModule,
    NzTableModule,
    NzButtonModule,
    NzModalModule,
    NzInputModule,
    ReactiveFormsModule,
    ReactiveFormsModule,
    NzBadgeModule,
    NzDividerModule,
    NzDropDownModule,
    NzIconModule,
    NzCheckboxModule,
    NzFormModule,
    NzSelectModule,
    HospitalRoutingModule,
  ],
})
export class PatientModule {}
