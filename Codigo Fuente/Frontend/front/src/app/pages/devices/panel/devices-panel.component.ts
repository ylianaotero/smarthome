import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GetDeviceResponse} from '../../../interfaces/devices';
import { ApiDeviceService } from '../../../shared/devices.service';
import {DeviceFilterRequestModel} from './model-device';
import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';

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

  user : userRetrieveModel | null = null;

  constructor(private router: Router, private deviceApi : ApiDeviceService) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
    this.userName = this.user?.name || 'Usuario';
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
    this.deviceApi.getDevices(filters, this.currentPage, this.pageSize).subscribe({
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
    this.deviceApi.getSupportedDevices().subscribe({
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
