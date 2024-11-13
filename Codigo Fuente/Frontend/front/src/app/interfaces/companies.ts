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

export interface PostCompaniesResponse {
  name: string | null;
  rut: string | null;
  logoUrl: string | null;
  ownerId: number | null;
  validateNumber: boolean | null;
}
