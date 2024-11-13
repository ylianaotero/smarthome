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

  currentPage: number = 1;
  pageSize: number = 1;

  totalUsers: number = 0;
  totalCompanies: number = 0;

  selectedFullName: string = '';
  selectedRole: string = '';
  selectedCompanyName: string = '';
  selectedOwner: string = '';
  selectedRUT: string = '';
  selectedEmail: string = '';
  selectedPhotoURL: string | null = '';

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];

  modalShowUsers: boolean = false;
  modalShowCompanies: boolean = false;

  constructor(private router: Router, private api: AdministratorService, private apiCompany: CompanyService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    //this.getUsers();
    this.getCompany();
  }

  ownerHasCompany(): boolean {
    return this.companies.length > 0;
  }

  getCompany(): void {
    const request: GetCompaniesRequest = {
      name: "",
      owner: this.userName
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

  goCreateCompany(): void {
    this.router.navigate(['/administrator/new-admin']);
  }

  goViewCompany(): void {
    this.router.navigate(['/administrator/new-admin']);
  }

  goViewCompanyDevices(): void {
    this.router.navigate(['/administrator/new-admin']);
  }

  goCreateDevices(): void {
    this.router.navigate(['/administrator/new-admin']);
  }


  changePage(page: number): void {
    this.currentPage = page;
  }
  get totalPages(): number {
    return Math.ceil(this.totalUsers / this.pageSize);
  }

  openModal(modal: string) {
    if (modal === 'modalShowCompanies') {
      this.modalShowCompanies = true;
    }
  }

  closeModal(modal: string) {
    if (modal === 'showCompanies') {
      this.modalShowCompanies = false;
    }
  }

  closeModalBackdrop(event: MouseEvent, modal: string) {
    if ((event.target as HTMLElement).classList.contains('modal')) {
      this.closeModal(modal);
    }
  }
}
