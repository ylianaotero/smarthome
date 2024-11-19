
export interface GetCompaniesResponse {
    companies: GetCompanyResponse[];
}

export interface GetCompanyResponse {
    id: number;
    owner: string;
    ownerEmail: string;
    name: string;
    rut: string;
    logoURL: string | null;
}

export class CreateCompanyRequest {
  name: string | null;
  rut: string | null;
  logoUrl: string | null;
  ownerId: number | null;
  ValidationMethod: string | null;

  constructor(name: string, rut: string, logoUrl: string, ownerId: number, validation: string) {
    this.name = name;
    this.rut = rut;
    this.logoUrl = logoUrl;
    this.ownerId = ownerId;
    this.ValidationMethod = validation;
  }
}

export interface PostCompaniesResponse {
  name: string | null;
  rut: string | null;
  logoUrl: string | null;
  ownerId: number | null;
  ValidationMethod: string | null;

}
