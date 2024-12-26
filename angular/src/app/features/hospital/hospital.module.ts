import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HospitalRoutingModule } from './hospital-routing.module';
import { NzTableModule } from 'ng-zorro-antd/table';
import { SharedModule } from 'src/app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ListHospitalComponent } from './component/list-hospital/list-hospital.component';
import { AddHospitalComponent } from './component/add-hospital/add-hospital.component';
import { UpdateHospitalComponent } from './component/update-hospital/update-hospital.component';
import { AddUserHospitalComponent } from './component/add-user-hospital/add-user-hospital.component';

@NgModule({
  declarations: [
    ListHospitalComponent,
    AddHospitalComponent,
    UpdateHospitalComponent,
    AddUserHospitalComponent
  ],
  imports: [
    CommonModule,
    NzTableModule,
    SharedModule,
    ReactiveFormsModule,
    NzBadgeModule,
    NzDividerModule,
    NzDropDownModule,
    NzIconModule,
    NzTableModule,
    NzButtonModule,
    NzModalModule,
    NzCheckboxModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    HospitalRoutingModule,
  ],
})
export class HospitalModule {}
