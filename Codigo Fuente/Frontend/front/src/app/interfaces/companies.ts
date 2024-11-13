export interface GetCompaniesRequest {
    name: string | null;
    owner: string | null;
    ownerEmail: string | null;
}

export interface GetCompaniesResponse {
    companies: GetCompanyResponse[];
}

export interface GetCompanyResponse {
    owner: string;
    ownerEmail: string;
    name: string;
    rut: string;
    logoURL: string | null;
}

export interface PostCompaniesRequest {
  name: string | null;
  rut: string | null;
  logoUrl: string | null;
  ownerId: number | null;
}

export class CreateCompanyRequest {
  name: string | null;
  rut: string | null;
  logoUrl: string | null;
  ownerId: number | null;

  constructor(name: string, rut: string, logoUrl: string, ownerId: number) {
    this.name = name;
    this.rut = rut;
    this.logoUrl = logoUrl;
    this.ownerId = ownerId;
  }
}

export interface PostCompaniesResponse {
  name: string | null;
  rut: string | null;
  logoUrl: string | null;
  ownerId: number | null;
  validateNumber: boolean | null;
}
