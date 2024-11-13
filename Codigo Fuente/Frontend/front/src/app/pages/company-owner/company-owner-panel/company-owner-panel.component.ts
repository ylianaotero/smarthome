import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdministratorService } from '../../../shared/administrator.service';
import { CompanyService } from '../../../shared/company.service';
import { GetCompaniesRequest, GetCompaniesResponse, GetCompanyResponse } from '../../../interfaces/companies';
import { GetUsersRequest, GetUsersResponse, GetUserResponse} from '../../../interfaces/users';

@Component({
  selector: 'app-company-owner-panel',
  templateUrl: './company-owner-panel.component.html',
  styleUrl: './company-owner-panel.component.css'
})
export class CompanyOwnerPanelComponent implements OnInit {

  userName: string;
  userEmail: string;

  currentPage: number = 1;
  pageSize: number = 1;
  totalCompanies: number = 0;

  // Modal state for showing company details and creating company
  modalShowCompanies: boolean = false;
  modalCreateCompany: boolean = false;

  // Data for the selected company
  selectedCompanyName: string = '';
  selectedOwner: string = '';
  selectedRUT: string = '';
  selectedEmail: string = '';
  selectedPhotoURL: string | null = '';

  // New company data
  newCompany = {
    name: '',
    rut: '',
    logoUrl: '',
    ownerId: 0 // This will be set to the current user's ID
  };

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];

  constructor(private router: Router, private api: AdministratorService, private apiCompany: CompanyService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
    this.userEmail = this.api.currentSession?.user?.email || '';
  }

  ngOnInit(): void {
    this.getCompany();
    this.setOwnerId(); // Set the ownerId when the component initializes
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

  // Set the ownerId to the current user's ID
  setOwnerId(): void {
    this.newCompany.ownerId = this.api.currentSession?.user?.id || 0;
  }

  goViewDevices(): void {
    this.router.navigate(['home/devices-list']);
  }

  goCreateDevices(): void {
    this.router.navigate(['/administrator/new-admin']);
  }

  // Open modal for creating a new company
  openModal(modal: string) {
    if (modal === 'modalCreateCompany') {
      this.modalCreateCompany = true;
    }
    if (modal === 'modalShowCompanies') {
      this.modalShowCompanies = true;
    }
  }

  // Close modal
  closeModal(modal: string) {
    if (modal === 'createCompany') {
      this.modalCreateCompany = false;
    }
    if (modal === 'showCompanies') {
      this.modalShowCompanies = false;
    }
  }

  // Close modal when clicking outside the modal (backdrop)
  closeModalBackdrop(event: MouseEvent, modal: string) {
    if ((event.target as HTMLElement).classList.contains('modal')) {
      this.closeModal(modal);
    }
  }

  // Handle the company creation form submission
  createCompany(): void {
    this.apiCompany.createCompany(this.newCompany).subscribe({
      next: (response) => {
        console.log('Company created successfully', response);
        this.closeModal('createCompany');
        this.getCompany(); // Refresh the list of companies
      },
      error: (err) => {
        console.error('Error creating company', err);
      }
    });
  }
}
