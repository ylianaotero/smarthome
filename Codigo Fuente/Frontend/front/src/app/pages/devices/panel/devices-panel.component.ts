import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GetDeviceRequest, GetDeviceResponse, GetDevicesResponse, GetDeviceTypesResponse } from '../../../interfaces/devices';
import { AdministratorService } from '../../../shared/administrator.service';
import { DevicesService } from '../../../shared/devices.service';
import {ApiService} from '../../../shared/api.service';
import {DeviceFilterRequestModel} from './model-device';

@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-panel.component.html',
  styleUrls: ['./devices-panel.component.css', '../../../../styles.css']
})
export class DevicesPanelComponent implements OnInit {
  userName: string;
  devices: GetDeviceResponse[] = [];
  deviceTypes: string[] = [];

  selectedName: string = '';
  selectedModel: string = '';
  selectedCompany: string = '';
  selectedKind: string = '';

  modalDevice: string | null = '';
  modalImage: string | null = null;
  isModalOpen = false;

  constructor(private router: Router, private sharedApi: ApiService) {
    this.userName = this.sharedApi.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getDevices();
    this.getSupportedDevices();
  }

  currentPage: number = 1;
  pageSize: number = 6; //cuantos se van a ver por pagina
  totalDevices: number = 0;

  getDevices(): void {

    const filters = new DeviceFilterRequestModel(
      this.selectedName,
      this.selectedModel,
      this.selectedCompany,
      this.selectedKind
    );
    this.sharedApi.getDevices(filters, this.currentPage, this.pageSize).subscribe({
      next: (res: any) => {
        this.devices = res.devices || [];
        this.totalDevices =  res.totalCount || 0;
      },
      error: (err) => {
        console.error('Error al obtener dispositivos', err);
      }
    });
  }


  changePage(page: number): void {
    this.currentPage = page;
    this.getDevices();
  }
  get totalPages(): number {
    return Math.ceil(this.totalDevices / this.pageSize);
  }



  getSupportedDevices(): void {
    this.sharedApi.getSupportedDevices().subscribe({
      next: (res: any) => {
        this.deviceTypes = res.deviceTypes || [];
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
