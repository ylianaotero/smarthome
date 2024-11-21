import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {GetNotificationResponse, NotificationsFilterRequestModel} from './model-notification';
import {ApiDeviceService} from '../../../shared/devices.service';
import {ApiNotificationService} from '../../../shared/notification.service';


@Component({
  selector: 'app-notifications-list',
  templateUrl: './notifications-panel.component.html',
  styleUrls: [ '../../../../styles.css']
})
export class NotificationsPanelComponent implements OnInit {
  constructor(private router: Router, private api: ApiNotificationService, private deviceApi : ApiDeviceService) {
    console.log("Componente creado");
  }

  notifications: GetNotificationResponse[] = [];

  selectedKind : string = '';

  deviceTypes: string[] = [];

  selectedRead : boolean = false;

  selectedDate: Date | null = null;


  ngOnInit(): void {
    this.getNotifications();
    this.getSupportedDevices();
  }

  getSupportedDevices(): void {
    this.deviceApi.getSupportedDevices().subscribe({
      next: (res: any) => {
        this.deviceTypes = res.deviceTypes || [];
      }
    });
  }

  getNotifications(): void {
    const filters = new NotificationsFilterRequestModel(
      this.selectedRead,
      this.selectedKind,
      this.selectedDate
    );
    this.api.getNotifications(filters).subscribe({
      next: (res: any) => {
        console.log(res);
        this.notifications = res.notifications || [];
      },
      error: (err) => {
        console.error('Error al obtener dispositivos', err);
      }
    });
  }
}
