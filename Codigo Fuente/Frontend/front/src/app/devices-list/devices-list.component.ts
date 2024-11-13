import { Component } from '@angular/core';
import {GetDeviceRequest, GetDeviceResponse, GetDevicesResponse} from '../interfaces/devices';
import {Router} from '@angular/router';
import {AdministratorService} from '../shared/administrator.service';
import {DevicesService} from '../shared/devices.service';

interface Device {
  name: string;
  model: string;
  photoUrl: string;
  companyName: string;
}

@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
  styleUrls: ['./devices-list.component.css']
})


export class DevicesListComponent {
  ngOnInit(): void {
    this.getDevices();
  }

  userName: string;
  constructor(private router: Router, private api: AdministratorService, private apiDevices : DevicesService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }


  devices: GetDeviceResponse[] = [];

  modalDevice: string | null = '';

  modalImage: string | null = null;
  isModalOpen = false;

  selectedName: string = '';
  selectedModel: string = '';
  selectedCompany: string = '';
  selectedKind: string = '';

  totalDevices: number = 0;
  currentPage: number = 1;
  pageSize: number = 1;

  getDevices(): void {
    const request: GetDeviceRequest = {
      name: this.selectedName,
      model: this.selectedModel,
      company: this.selectedCompany,
      kind: this.selectedKind
    }

    this.apiDevices.getDevices(request).subscribe({
      next: (res: GetDevicesResponse) => {
        this.devices = res.devices || [];
        this.totalDevices = res.devices.length;
        console.log(this.devices);
      }
    });
  }

  // Open modal and set image
  openModal(imageUrl: string | null, device: string | null) {
    this.modalDevice = device;
    this.modalImage = imageUrl;
    this.isModalOpen = true;
  }

  // Close modal
  closeModal() {
    this.isModalOpen = false;
  }

  doNothing() {
    // Do nothing
  }
}
