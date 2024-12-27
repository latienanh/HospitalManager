import { Component } from '@angular/core';
import { NotificationAddPatientService } from './core/service/notification-add-patient.service';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
  `,
})
export class AppComponent {
  constructor(private notificationAddPatientService: NotificationAddPatientService){

  }
}
