import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GetDeviceRequest, GetDeviceResponse, GetDevicesResponse, GetDeviceTypesResponse } from '../interfaces/devices';
import { AdministratorService } from '../shared/administrator.service';
import { DevicesService } from '../shared/devices.service';

@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
  styleUrls: ['./devices-list.component.css', '../../styles.css']
})
export class DevicesListComponent implements OnInit {
  userName: string;
  devices: GetDeviceResponse[] = [];
  deviceTypes: string[] = [];
  totalDevices: number = 0;
  currentPage: number = 1;
  pageSize: number = 1;

  selectedName: string = '';
  selectedModel: string = '';
  selectedCompany: string = '';
  selectedKind: string = '';

  modalDevice: string | null = '';
  modalImage: string | null = null;
  isModalOpen = false;

  constructor(private router: Router, private api: AdministratorService, private apiDevices: DevicesService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getDevices();
    this.getDevicesTypes();
  }

  getDevices(): void {
    const request: GetDeviceRequest = {
      name: this.selectedName,
      model: this.selectedModel,
      company: this.selectedCompany,
      kind: this.selectedKind
    };

    this.apiDevices.getDevices(request).subscribe({
      next: (res: GetDevicesResponse) => {
        this.devices = res.devices || [];
        this.totalDevices = res.devices.length;
        console.log(this.devices);
      }
    });
  }

  getDevicesTypes(): void {
    this.apiDevices.getDeviceTypes().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        this.deviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }

  openModal(imageUrl: string | null, device: string | null): void {
    this.modalDevice = device;
    this.modalImage = imageUrl;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }
}
