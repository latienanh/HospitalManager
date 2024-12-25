import { Component, EventEmitter, Input, Output } from '@angular/core';
import { VisibleImportProvince } from '../../models/visibleaddprovince';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProvinceService } from '../../../../proxy/services/province.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { createIFormFile } from 'src/app/features/common/create-iformfile';
import { IFormFile } from '@proxy/microsoft/asp-net-core/http';

@Component({
  selector: 'app-import-province-excel',
  templateUrl: './import-province-excel.component.html',
  styleUrls: ['./import-province-excel.component.scss'],
})
export class ImportProvinceExcelComponent {
  @Input() isVisibleImportProvince: VisibleImportProvince;
  @Output() visibilityChange = new EventEmitter<VisibleImportProvince>();
  provinceImportForm: FormGroup;
  fileList: NzUploadFile[] = [];

  constructor(
    private fb: FormBuilder,
    private provinceService: ProvinceService,
    private notification: NzNotificationService
  ) {
    this.provinceImportForm = this.fb.group({
      isUpdate: [false, [Validators.requiredTrue]],
    });
  }

  beforeUpload = (file: NzUploadFile): boolean => {
    this.fileList = [file];
    return false; // Prevent automatic upload
  };

  handleCancel() {
    this.visibilityChange.emit({ importProvinceStatus: false, showImportProvinceForm: false });
  }

  submitForm(): void {
    if (this.fileList.length === 0) {
      this.notification.error('Lỗi', 'Vui lòng chọn một tệp để tải lên.');
      return;
    }

    if (this.provinceImportForm.valid) {
      const formValue = this.provinceImportForm.value;
      const nzFile = this.fileList[0];
      const isUpdate = formValue.isUpdate;

      if (nzFile.originFileObj instanceof File) {
        const customFormFile: IFormFile = createIFormFile(nzFile.originFileObj);
        
        this.provinceService.importExcelByFileAndIsUpdate(customFormFile, isUpdate).subscribe({
          next: (response) => {
            console.log(response);
            this.notification.success('Thành công', 'Tệp đã được tải lên thành công');
            this.visibilityChange.emit({ importProvinceStatus: true, showImportProvinceForm: false });
          },
          error: (err) => {
            console.error(err);
            this.notification.error('Lỗi', 'Đã xảy ra lỗi khi tải lên tệp');
          }
        });
      } else {
        this.notification.error('Lỗi', 'Không thể đọc tệp đã chọn.');
      }
    } else {
      this.notification.error('Lỗi', 'Form không hợp lệ. Vui lòng điền đầy đủ thông tin bắt buộc.');
    }
  }
}

