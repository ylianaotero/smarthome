import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  CreateCompanyRequest,
  GetCompaniesResponse, GetCompanyRequest,
  GetCompanyResponse
} from '../../../interfaces/companies';
import { GetUserResponse } from '../../../interfaces/users';
import {GetDeviceTypesResponse} from '../../../interfaces/devices';
import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';
import {ApiDeviceService} from '../../../shared/devices.service';
import {ApiCompanyService} from '../../../shared/company.service';

@Component({
  selector: 'app-company-owner-panel',
  templateUrl: './company-owners-panel.component.html',
  styleUrls: ['../../../../styles.css']
})
export class CompanyOwnersPanelComponent implements OnInit {
  userName: string;
  userEmail: string;
  userId: number;

  currentPage: number = 1;
  pageSize: number = 1;
  totalCompanies: number = 0;

  modalShowCompanies: boolean = false;
  modalShowDevicesTypes: boolean = false;

  selectedCompanyName: string = '';
  selectedOwner: string = '';
  selectedRUT: string = '';
  selectedEmail: string = '';
  selectedPhotoURL: string | null = '';

  newCompanyName: string = '';
  newCompanyRUT: string = '';
  newCompanyLogoURL: string = '';
  validation : string = '';

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];

  feedback: string = '';
  companyCreatedCorrectly: boolean = false;

  deviceTypes: string[] = [];

  user : userRetrieveModel | null = null;


  constructor(private router: Router, private companyApi : ApiCompanyService, private deviceApi : ApiDeviceService) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
    this.userName = this.user?.name || 'Usuario';
    this.userEmail = this.user?.email || '';
    this.userId = this.user?.id || 0;
  }

  ngOnInit(): void {
    this.getCompany();
    this.getDevicesTypes();
  }

  logOut() : void {
    localStorage.setItem('user', JSON.stringify(null));
    localStorage.setItem('token', '');
    this.router.navigate(['']);
  }

  goViewNotifications(): void {
    this.router.navigate(['/notifications']);
  }

  ownerHasCompany(): boolean {
    return this.companies.length > 0;
  }

  getCompany(): void {
    const data: GetCompanyRequest = {
      userEmail: this.userEmail,
      name: null,
      fullName: null,
    };
    this.companyApi.getCompanies(data).subscribe({
      next: (res: GetCompaniesResponse) => {
        this.companies = res.companies || [];
        this.totalCompanies = res.companies.length;
        this.selectedCompanyName = this.companies[0].name;
        this.selectedOwner = this.companies[0].owner;
        this.selectedEmail = this.companies[0].ownerEmail;
        this.selectedRUT = this.companies[0].rut;
        this.selectedPhotoURL = this.companies[0].logoURL;

        console.log(this.companies);
      }
    });
  }


  getDevicesTypes(): void {
    this.deviceApi.getSupportedDevices().subscribe({
      next: (res: GetDeviceTypesResponse) => {
        console.log(res)
        this.deviceTypes = res.deviceTypes || [];
        console.log(res.deviceTypes);
      }
    });
  }

  goViewDevices(): void {
    this.router.navigate(['devices']);
  }

  goCreateDevices(): void {
    this.router.navigate(['company-owners/create-device']);
  }

  goImportDevices(): void {
    this.router.navigate(['company-owners/import-device']);
  }

  goCreateCompany(): void {
    this.router.navigate(['company-owners/create-company']);
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
    if(modal == "modalShowCompanies"){
      this.modalShowCompanies = showModal;
    }else if(modal == "showDevicesTypes"){
      this.modalShowDevicesTypes = showModal;
    }
  }

  closeModalBackdrop(event: MouseEvent,modal: string ): void {
    const target = event.target as HTMLElement;
    if (target.id === 'modalShowCompanies') {
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

  createCompany(): void {
    if (this.newCompanyName === '' || this.newCompanyRUT === '' || this.newCompanyLogoURL === '') {
      this.feedback = 'Por favor, completa todos los campos obligatorios.';
      return
    }

    this.companyApi
      .createCompany(new CreateCompanyRequest
      (this.newCompanyName, this.newCompanyRUT.toString(), this.newCompanyLogoURL, this.userId, this.validation))
      .subscribe({
      next: (response) => {
        console.log('Company created successfully', response);
        this.closeModal('createCompany');
        this.getCompany();
        this.feedback = 'La empresa fue creada con éxito.';
        this.companyCreatedCorrectly = true;
      },
      error: (err) => {
        console.error('Error creating company', err);
        if (err.status === 400) {
          this.feedback = 'Por favor, completa todos los campos obligatorios.';
          return
        }
        if (err.status === 401 || err.status === 403) {
          this.feedback = 'No tienes permisos para realizar esta acción.';
          return
        }
        if (err.status === 404) {
          this.feedback = 'No eres dueño de empresa o ya tienes una empresa asociada.';
          return
        }
        if (err.status === 415) {
          this.feedback = 'El formato no es válido.';
          return
        }
      }
    });
  }

}
