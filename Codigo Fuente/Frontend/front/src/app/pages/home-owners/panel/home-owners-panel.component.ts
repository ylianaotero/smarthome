import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../../shared/api.service';
import {DeviceFilterRequestModel, deviceModel} from '../../devices/panel/model-device';
import {GetDeviceResponse, GetDeviceTypesResponse} from '../../../interfaces/devices';

@Component({
  selector: 'app-account',
  templateUrl: './home-owners-panel.component.html',
  styleUrls: ['../../../../styles.css']
})
export class HomeOwnersPanelComponent implements OnInit {

  userName: string;

  supportedDevices!: string[];


  devices!: deviceModel[];

  selectedName: string = "";
  selectedCompany: string = "";
  selectedKind: string = "";
  selectedModel: string = "";


  modalShowDevices: boolean = false;
  modalShowDevicesTypes: boolean = false;

  deviceTypes: string[] = [];

  constructor(private api: ApiService, private router: Router) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getDevicesTypes();
  }

  getDevicesTypes(): void {
    this.api.getSupportedDevices().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        console.log(res)
        this.deviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }


  goCreateHome(): void {
    this.router.navigate(['/home-owners/homes/create']);
  }

  goHomesOfHomeOwner(): void {
    this.router.navigate(['/home-owners/homes']);
  }

  goViewDevices(): void {
    this.router.navigate(['/devices']);
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
