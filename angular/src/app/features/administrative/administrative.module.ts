import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ReactiveFormsModule } from '@angular/forms';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTableModule } from 'ng-zorro-antd/table';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdministrativeRoutingModule } from './administrative-routing.module';
import { AdministrativeComponent } from './component/administrative/administrative.component';
import { AddProvinceComponent } from './component/add-province/add-province.component';
import { AddDistrictComponent } from './component/add-district/add-district.component';
import { AddWardComponent } from './component/add-ward/add-ward.component';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule, NzFormTooltipIcon } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ImportProvinceExcelComponent } from './component/import-province-excel/import-province-excel.component';
@NgModule({
  declarations: [
    AdministrativeComponent,
    AddProvinceComponent,
    AddDistrictComponent,
    AddWardComponent,
    ImportProvinceExcelComponent,
  ],
  imports: [
    CommonModule,
    AdministrativeRoutingModule,
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
  ],
})
export class AdministrativeModule {}
