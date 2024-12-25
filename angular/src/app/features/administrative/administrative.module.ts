import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ReactiveFormsModule } from '@angular/forms';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdministrativeRoutingModule } from './administrative-routing.module';
import { AddDistrictComponent } from './component/add-district/add-district.component';
import { AddProvinceComponent } from './component/add-province/add-province.component';
import { AddWardComponent } from './component/add-ward/add-ward.component';
import { ImportProvinceExcelComponent } from './component/import-province-excel/import-province-excel.component';
import { ListDistrictComponent } from './component/list-district/list-district.component';
import { ListProvinceComponent } from './component/list-province/list-province.component';
import { ListWardComponent } from './component/list-ward/list-ward.component';
@NgModule({
  declarations: [
    AddProvinceComponent,
    AddDistrictComponent,
    AddWardComponent,
    ListProvinceComponent,
    ListDistrictComponent,
    ListWardComponent,
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
