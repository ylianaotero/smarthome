import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdministratorService } from '../../../shared/administrator.service';
import { CompanyService } from '../../../shared/company.service';
import {
  CreateCompanyRequest,
  GetCompaniesRequest,
  GetCompaniesResponse,
  GetCompanyResponse
} from '../../../interfaces/companies';
import { GetUserResponse } from '../../../interfaces/users';

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
  modalCreateCompany: boolean = false;

  selectedCompanyName: string = '';
  selectedOwner: string = '';
  selectedRUT: string = '';
  selectedEmail: string = '';
  selectedPhotoURL: string | null = '';

  newCompanyName: string = '';
  newCompanyRUT: string = '';
  newCompanyLogoURL: string = '';

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];

  feedback: string = '';
  companyCreatedCorrectly: boolean = false;

  constructor(private router: Router, private api: AdministratorService, private apiCompany: CompanyService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
    this.userEmail = this.api.currentSession?.user?.email || '';
    this.userId = this.api.currentSession?.user?.id || 0;
  }

  ngOnInit(): void {
    this.getCompany();
  }

  ownerHasCompany(): boolean {
    return this.companies.length > 0;
  }

  getCompany(): void {
    const request: GetCompaniesRequest = {
      name: "",
      owner: this.userName,
      ownerEmail: this.userEmail
    };

    this.apiCompany.getCompanies(request).subscribe({
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
    if (modal === 'modalShowCompanies') {
      this.modalShowCompanies = true;
    } else if (modal === 'modalCreateCompany') {
      this.modalCreateCompany = true;
    }
    document.body.classList.add('modal-open');
    this.createBackdrop();
  }

  closeModal(modal: string): void {
    if (modal === 'showCompanies') {
      this.modalShowCompanies = false;
    } else if (modal === 'createCompany') {
      this.modalCreateCompany = false;
    }
    this.feedback = '';
    document.body.classList.remove('modal-open');
    this.removeBackdrop();
  }

  closeModalBackdrop(event: MouseEvent, modal: string): void {
    if ((event.target as HTMLElement).classList.contains('modal')) {
      this.closeModal(modal);
    }
  }

  createCompany(): void {
    if (this.newCompanyName === '' || this.newCompanyRUT === '' || this.newCompanyLogoURL === '') {
      this.feedback = 'Por favor, completa todos los campos obligatorios.';
      return
    }

    this.apiCompany
      .createCompany(new CreateCompanyRequest
      (this.newCompanyName, this.newCompanyRUT.toString(), this.newCompanyLogoURL, this.userId))
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
