import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AdministratorService } from '../../../shared/administrator.service';
import { CompanyService } from '../../../shared/company.service';
import { GetCompanyResponse } from '../../../interfaces/companies';
import { GetUsersRequest, GetUsersResponse, GetUserResponse} from '../../../interfaces/users';
import {ApiService} from '../../../shared/api.service';

@Component({
  selector: 'app-administrator-panel',
  templateUrl: './administrator-panel.component.html',
  styleUrls: ['../../../../styles.css']
})
export class AdministratorPanelComponent implements OnInit {
  userName: string;

  currentPage: number = 1;
  pageSize: number = 1;

  totalUsers: number = 0;

  selectedFullName: string = '';
  selectedRole: string = '';

  users: GetUserResponse[] = [];
  companies: GetCompanyResponse[] = [];

  modalShowUsers: boolean = false;
  modalShowCompanies: boolean = false;

  constructor(private router: Router, private api: ApiService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  ngOnInit(): void {
    this.getUsers();
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

  goCreateUser(): void {
    this.router.navigate(['/administrators/create-user']);
  }

  goListUsers(): void {
    this.router.navigate(['/administrators/list-users']);
  }

  goDeleteAdmin(): void {
    this.router.navigate(['/administrators/delete-admin']);
  }

  goListCompanies(): void {
    this.router.navigate(['/administrators/list-companies']);
  }

  get totalPages(): number {
    return Math.ceil(this.totalUsers / this.pageSize);
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

  private removeBackdrop(): void {
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
      document.body.removeChild(backdrop);
    }
  }
}
