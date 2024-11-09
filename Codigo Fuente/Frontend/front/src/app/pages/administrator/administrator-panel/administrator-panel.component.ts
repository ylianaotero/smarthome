import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdministratorService } from '../../../shared/administrator.service';
import { CompanyService } from '../../../shared/company.service';
import { GetCompaniesRequest, GetCompaniesResponse, GetCompanyResponse } from '../../../interfaces/companies';
import { GetUsersRequest, GetUsersResponse, GetUserResponse} from '../../../interfaces/administrator';

@Component({
  selector: 'app-administrator-panel',
  templateUrl: './administrator-panel.component.html',
  styleUrl: './administrator-panel.component.css'
})
export class AdministratorPanelComponent implements OnInit {

  userName: string;

  currentPage: number = 1;
  pageSize: number = 1;

  totalUsers: number = 0;
  totalCompanies: number = 0;

  selectedFullName: string = '';
  selectedRole: string = '';
  selectedCompanyName: string = '';
  selectedOwner: string = '';

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];

  modalShowUsers: boolean = false;
  modalShowCompanies: boolean = false;

  constructor(private router: Router, private api: AdministratorService, private apiCompany: CompanyService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    //this.getUsers();
    this.getCompanies();
  }
  
  getUsers(): void {
    const request: GetUsersRequest = {
      fullName: this.selectedFullName,
      role: this.selectedRole
    };

    this.api.getUsers(request).subscribe({
      next: (res: GetUsersResponse) => {
        this.users = res.users || [];
        this.totalUsers = res.users.length;
        console.log(this.users);
      }
    });
  }


  getCompanies(): void {
    const request: GetCompaniesRequest = {
      name: this.selectedCompanyName,
      owner: this.selectedOwner
    };

    this.apiCompany.getCompanies(request).subscribe({
      next: (res: GetCompaniesResponse) => {
        this.companies = res.companies || [];
        this.totalCompanies = res.companies.length;
        console.log(this.companies);
      }
    });
  }

  goCreateAdmin(): void {
    this.router.navigate(['/administrator/new-admin']);
  }

  //Aca cambio de pagina
  changePage(page: number): void {
    this.currentPage = page;
    this.getUsers();
  }
  get totalPages(): number {
    return Math.ceil(this.totalUsers / this.pageSize);
  }

  //modales
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
    if(modal == "showUsers"){
      this.modalShowUsers = showModal;
    }else if(modal == "showCompanies"){
      this.modalShowCompanies = showModal;
    }
  }

  closeModalBackdrop(event: MouseEvent,modal: string ): void {
    const target = event.target as HTMLElement;
    if (target.id === 'myModalShowUsers') {
      this.closeModal(modal);
    }else if(target.id === 'myModalShowCompanies'){
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
