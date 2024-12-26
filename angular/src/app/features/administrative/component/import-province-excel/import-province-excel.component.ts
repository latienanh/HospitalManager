import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProvinceService } from '../../../../proxy/services/province.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { createIFormFile } from 'src/app/features/common/create-iformfile';
import { IFormFile } from '@proxy/microsoft/asp-net-core/http';
import { NzMessageService } from 'ng-zorro-antd/message';
import { VisibleImport } from '../../models/visible';

@Component({
  selector: 'app-import-province-excel',
  templateUrl: './import-province-excel.component.html',
  styleUrls: ['./import-province-excel.component.scss'],
})
export class ImportProvinceExcelComponent {
  @Input() isVisibleImportProvince: VisibleImport;
  @Output() visibilityChange = new EventEmitter<VisibleImport>();
  provinceImportForm: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private provinceService: ProvinceService,
    private notification: NzNotificationService,
    private messageService: NzMessageService
  ) {
    this.provinceImportForm = this.fb.group({
      isUpdate: [false],
    });
  }

  onFileChange(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      console.log('Selected file:', file);
    }
  }

  handleCancel() {
    this.visibilityChange.emit({ importStatus: false, showImportForm: false });
  }

  submitForm(): void {
    if (!this.selectedFile) {
      this.notification.error('Lỗi', 'Vui lòng chọn một tệp để tải lên.');
      return;
    }

    if (this.provinceImportForm.valid) {
      const formValue = this.provinceImportForm.value;
      const isUpdate = formValue.isUpdate;
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);
      const customFormFile: IFormFile = createIFormFile(this.selectedFile);
        
      this.provinceService.importExcelByFileAndIsUpdate(formData, isUpdate).subscribe({
        next: (response) => {
          console.log(response);
          this.notification.success('Thành công', 'Tệp đã được tải lên thành công');
          this.provinceImportForm.reset();
          this.selectedFile = null;
          this.visibilityChange.emit({ importStatus: true, showImportForm: false });
        },
        error: (err) => {
          console.error(err);
          this.notification.error('Lỗi', 'Đã xảy ra lỗi khi tải lên tệp');
        }
      });
    } else {
      this.notification.error('Lỗi', 'Form không hợp lệ. Vui lòng điền đầy đủ thông tin bắt buộc.');
    }
  }
}
