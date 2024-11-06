export interface GetCompaniesRequest {
    name: string | null;
    owner: string | null;
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