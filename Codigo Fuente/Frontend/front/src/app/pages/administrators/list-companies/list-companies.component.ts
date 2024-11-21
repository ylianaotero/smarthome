import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {GetCompanyResponse, GetCompaniesResponse, GetCompanyRequest} from '../../../interfaces/companies';
import {ApiCompanyService} from '../../../shared/company.service';

@Component({
  selector: 'app-list-companies',
  templateUrl: './list-companies.component.html',
  styleUrls: ['../../../../styles.css'],
})
export class ListCompaniesComponent implements OnInit{
  companies: GetCompanyResponse[] = [];
  totalCompanies: number = 0;
  selectedCompanyName: string = "";
  selectedOwnerName: string = "";
  selectedOwnerEmail: string = "";

  feedback: string = "";

  modalCompany: string | null = '';
  modalImage: string | null = null;
  isPhotoModalOpen = false;

  currentPage: number = 1;
  pageSize: number = 6; //cuantos se van a ver por pagina

  constructor(private api: ApiCompanyService, private router: Router) {}

  ngOnInit(): void {
    this.getCompanies();
  }

  getCompanies(): void {
    const data: GetCompanyRequest = {
      userEmail: this.selectedOwnerEmail,
      name: this.selectedCompanyName,
      fullName: this.selectedOwnerName,
    };
    this.api.getCompanies(data, this.currentPage, this.pageSize).subscribe({
      next: (res: GetCompaniesResponse) => {
        this.companies = res.companies || [];
        this.totalCompanies = res.totalCount;
        console.log(this.companies);
      }
    });
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.getCompanies();
  }
  get totalPages(): number {
    return Math.ceil(this.totalCompanies / this.pageSize);
  }

  formatDate(date: string): string {
    let splittedDate = date.split('T');
    let time = splittedDate[1].split('.');
    return splittedDate[0] + ' ' + time[0];
  }

  userPhotoUrl(company: GetCompanyResponse): string {
    return company.logoURL ? company.logoURL : 'https://i.postimg.cc/hP6nR6FY/smarthomeuser.webp'
  }

  openPhotoModal(imageUrl: string | null, user: string | null): void {
    this.modalCompany = user;
    this.modalImage = imageUrl;
    this.isPhotoModalOpen = true;
  }

  closeModal(): void {
    this.isPhotoModalOpen = false;
  }
}
