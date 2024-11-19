import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { AdministratorService } from '../../shared/administrator.service';
import { GetDeviceRequest, GetDevicesResponse, GetDeviceResponse, GetDeviceTypesResponse } from '../../interfaces/devices';
import { DevicesService } from '../../shared/devices.service';
import {ApiService} from '../../shared/api.service';
import {Implement} from '@angular/cli/lib/config/workspace-schema';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrl: './user-panel.component.css'
})
export class UserPanelComponent implements OnInit{

  userName: string;

  selectedName: string = '';
  selectedModel: string = '';
  selectedCompany: string = '';
  selectedKind: string = '';

  modalShowDevices: boolean = false;
  modalShowDevicesTypes: boolean = false;

  totalDevices: number = 0;
  currentPage: number = 1;
  pageSize: number = 1;

  devices : GetDeviceResponse[] = [];
  deviceTypes: string[] = [];

  constructor(private router: Router, private api: ApiService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getDevicesTypes();
  }



  getDevicesTypes(): void {
    this.api.getDeviceTypes().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        this.deviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }

  changePage(page: number): void {
    this.currentPage = page;
  }

  get totalPages(): number {
    return Math.ceil(this.totalDevices / this.pageSize);
  }

  openModal(modal: string): void {
    this.changeSelectedModal(modal, true);
    document.body.classList.add('modal-open');
    this.createBackdrop();
  }

  closeModal(modal: string): void {
    this.changeSelectedModal(modal, false);
    document.body.classList.remove('modal-open');
    this.removeBackdrop();
  }

  changeSelectedModal(modal: string, showModal: boolean): void{
    if(modal == "showDevices"){
      this.modalShowDevices = showModal;
    }else if(modal == "showDevicesTypes"){
      this.modalShowDevicesTypes = showModal;
    }
  }

  closeModalBackdrop(event: MouseEvent,modal: string ): void {
    const target = event.target as HTMLElement;
    if (target.id === 'myModalShowDevices') {
      this.closeModal(modal);
    }else if(target.id === 'myModalShowDevicesTypes'){
      this.closeModal(modal);
    }
  }

  private createBackdrop(): void {
    const backdrop = document.createElement('div');
    backdrop.className = 'modal-backdrop fade show';
    document.body.appendChild(backdrop);
  }

  private removeBackdrop(): void {
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
      document.body.removeChild(backdrop);
    }
  }
}
