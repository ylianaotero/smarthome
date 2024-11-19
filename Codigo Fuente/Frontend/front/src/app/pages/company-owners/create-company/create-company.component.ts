import { Component } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Router } from '@angular/router';
import {
  CreateCompanyRequest,
  GetCompaniesResponse, GetCompanyRequest,
  GetCompanyResponse
} from '../../../interfaces/companies';
import { CompanyService } from '../../../shared/company.service';

@Component({
  selector: 'app-create-company',
  templateUrl: './create-company.component.html',
  styleUrls: ['../../../../styles.css']
})
export class CreateCompanyComponent {

  feedback: string = "";
  companyCreatedCorrectly: boolean = false;

  companyOwnerName: string = '';
  companyOwnerEmail: string = '';
  companyOwnerId: number;

  validation : string = '';

  newCompanyName: string = '';
  newCompanyRUT: string = '';
  newCompanyLogoURL: string = '';

  companies: GetCompanyResponse[] = [];

  constructor(private api: ApiService, private router: Router) {
    this.companyOwnerName = this.api.currentSession?.user?.name || 'Usuario';
    this.companyOwnerEmail = this.api.currentSession?.user?.email || '';
    this.companyOwnerId = this.api.currentSession?.user?.id || -1;
  }

  ngOnInit(): void {
    this.getCompany();
  }

  getCompany(): void {
    const data: GetCompanyRequest = {
      userEmail: this.companyOwnerEmail,
      name: null,
      fullName: null,
    };
    this.api.getCompanies(data).subscribe({
      next: (res: GetCompaniesResponse) => {
        this.companies = res.companies || [];
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/company-owners']);
  }

  createCompany(): void {
    if (this.newCompanyName === '' || this.newCompanyRUT === '' || this.newCompanyLogoURL === '') {
      this.feedback = 'Por favor, completa todos los campos obligatorios.';
      return
    }

    this.api
      .createCompany(new CreateCompanyRequest
      (this.newCompanyName, this.newCompanyRUT.toString(), this.newCompanyLogoURL, this.companyOwnerId, this.validation))
      .subscribe({
        next: (response) => {
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

