import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {GetCompanyResponse, GetCompaniesResponse} from '../../../interfaces/companies';
import {CompanyService} from '../../../shared/company.service';
import {ApiService} from '../../../shared/api.service';

@Component({
  selector: 'app-list-companies',
  templateUrl: './list-companies.component.html',
  styleUrls: ['../../../../styles.css'],
})
export class ListCompaniesComponent {
  companies: GetCompanyResponse[] = [];
  totalCompanies: number = 0;
  selectedCompanyName: string = "";
  selectedOwnerName: string = "";
  selectedOwnerEmail: string = "";

  feedback: string = "";

  modalCompany: string | null = '';
  modalImage: string | null = null;
  isPhotoModalOpen = false;

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.getCompanies();
  }

  getCompanies(): void {
    this.api.getCompanies(this.selectedOwnerEmail).subscribe({
      next: (res: GetCompaniesResponse) => {
        this.companies = res.companies || [];
        this.totalCompanies = res.companies.length;
        console.log(this.companies);
      }
    });
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
