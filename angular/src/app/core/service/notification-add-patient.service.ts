import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationAddPatientService {
  private hubConnection: signalR.HubConnection;
  protected readonly oAuthService = inject(OAuthService);

  constructor(private notification: NzNotificationService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:44348/notificationHub`, {
        accessTokenFactory: () => this.oAuthService.getAccessToken(),
      })
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error while starting SignalR connection: ' + err));

    // Nhận thông tin nhóm từ server
    this.hubConnection.on('ReceiveGroupInfo', (groupName: string) => {
      this.displayGroupInfo(groupName);
    });

    this.hubConnection.on('ReceiveNotification', (message: string) => {
      this.displayNotification(message);
    });
  }

  public sendNotification(message: string): void {
    this.hubConnection.invoke('SendNotification', message)
      .then(() => console.log('Notification sent: ', message))
      .catch(err => console.error('Error while sending notification: ', err));
  }

  private displayNotification(message: string) {
    this.notification.create('success', 'Thông báo', `${message}`);
  }

  private displayGroupInfo(groupName: string) {
    if (groupName) {
      console.log(`Bạn đang ở trong nhóm: ${groupName}`);

    } else {
      console.log('Bạn không thuộc nhóm nào.');
    }
  }
}
