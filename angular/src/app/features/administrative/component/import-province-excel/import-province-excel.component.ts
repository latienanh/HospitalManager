import { Component, EventEmitter, Input, Output } from '@angular/core';
import { VisibleImportProvince } from '../../models/visibleaddprovince';

@Component({
  selector: 'app-import-province-excel',
  templateUrl: './import-province-excel.component.html',
  styleUrl: './import-province-excel.component.scss'
})
export class ImportProvinceExcelComponent {
  @Input() isVisibleAddProvince: VisibleImportProvince ;
  @Output() visibilityChange = new EventEmitter<VisibleImportProvince>();
  handleCancel(){

  }
  submitForm(){
    
  }
}
