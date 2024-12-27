import { Component, EventEmitter, Input, Output } from '@angular/core';
import { VisibleImport } from '../../models/visible';
import { FormBuilder, FormGroup } from '@angular/forms';
import { WardService } from '@proxy/services';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { ErrorResponse } from 'src/app/core/models/error-response.model';

@Component({
  selector: 'app-import-ward-excel',
  templateUrl: './import-ward-excel.component.html',
  styleUrl: './import-ward-excel.component.scss',
})
export class ImportWardExcelComponent {
  @Input() isVisibleImportWard: VisibleImport;
  @Output() visibilityChange = new EventEmitter<VisibleImport>();
  wardImportForm: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private wardService: WardService,
    private notification: NzNotificationService
  ) {
    this.wardImportForm = this.fb.group({
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

    if (this.wardImportForm.valid) {
      const formValue = this.wardImportForm.value;
      const isUpdate = formValue.isUpdate;
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile.name);

      this.wardService.importExcelByFileAndIsUpdate(formData, isUpdate).subscribe({
        next: response => {
          console.log(response);
          this.notification.success('Thành công', 'Tệp đã được tải lên thành công');
          this.wardImportForm.reset();
          this.selectedFile = null;
          this.visibilityChange.emit({ importStatus: true, showImportForm: false });
        },
        error: (err: ErrorResponse) => {
          console.error(err);
          if (err.status == 403) {
            this.notification.error('Lỗi', `${err.error.error.data.message}`);
          } else {
            this.notification.error('Lỗi', 'Đã xảy ra lỗi khi tải lên tệp');
          }
        },
      });
    } else {
      this.notification.error('Lỗi', 'Form không hợp lệ. Vui lòng điền đầy đủ thông tin bắt buộc.');
    }
  }
}
