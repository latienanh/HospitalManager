import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { NotificationAddPatientService } from 'src/app/core/service/notification-add-patient.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService,private notificationAddPatientService: NotificationAddPatientService) {}

  login() {
    this.authService.navigateToLogin();
  }
}
