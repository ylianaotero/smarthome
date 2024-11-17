import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../../shared/api.service';
import {deviceUnit, home, member} from '../homes/panel/homeModels';
import {DeviceFilterRequestModel, deviceModel} from './deviceModels';

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
  selectedModel?: number;


  isModalOfListOfSupportedDevicesOpen: boolean = false;

  isModalOfListOfDevicesOpen: boolean = false;
  constructor(private api: ApiService, private router: Router) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getSupportedDevices();
    this.getDevices();
  }

  getSupportedDevices(): void {
    this.api.getSupportedDevices().subscribe({
      next: (res: any) => {
        this.supportedDevices = res.deviceTypes || [];
      }
    });
  }

  currentPage: number = 1;
  pageSize: number = 2; //cuantos se van a ver por pagina
  totalDevices: number = 0;

  getDevices(): void {
    const filters: DeviceFilterRequestModel = {
      Name: this.selectedName,
      Model: this.selectedModel,
      Company: this.selectedCompany,
      Kind: this.selectedKind
    };

    this.api.getDevices(filters, this.currentPage, this.pageSize).subscribe({
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

  goCreateHome(): void {
    this.router.navigate(['/home-owners/homes/create']);
  }

  goHomesOfHomeOwner(): void {
    this.router.navigate(['/home-owners/homes']);
  }

  goViewDevices(): void {
    this.router.navigate(['devices']);
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

  changeSelectedModal(modal: string, bool: boolean): void{
    if(modal == "showSupportedDevices"){
      this.isModalOfListOfSupportedDevicesOpen = bool;
    }else{
      if(modal == "showDevices"){
        this.isModalOfListOfDevicesOpen = bool;
      }
    }
  }

  closeModalBackdrop(event: MouseEvent,modal: string ): void {
    const target = event.target as HTMLElement;
    if (target.id === 'myModalShowSupportedDevices' || target.id === 'myModalShowDevices') {
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
