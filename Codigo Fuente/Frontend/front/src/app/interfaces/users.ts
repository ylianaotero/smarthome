export class createAdministratorModel {
    name: string;
    email: string;
    password: string;
    surname: string;
    photo: string;

    constructor(name: string, email: string, password: string, surname: string, photo: string) {
        this.name = name;
        this.email = email;
        this.password = password;
        this.surname = surname;
        this.photo = photo;
    }
}

export class addRole{
  Role: string;

  constructor(role: string) {
    this.Role = role;
  }
}

export interface PostAdministratorRequest {
    name: string;
    email: string;
    password: string;
    surname: string;
    photo: string;
}

export interface PostAdministratorResponse {
    name: string;
    email: string;
    surname: string;
}

export class createCompanyOwnerModel {
    name: string;
    email: string;
    password: string;
    surname: string;

    constructor(name: string, email: string, password: string, surname: string) {
        this.name = name;
        this.email = email;
        this.password = password;
        this.surname = surname;
    }
}

export interface ResponseAdmin {
  name: string;
  email: string;
  surname: string;
  id: number;
}

export interface PostCompanyOwnerRequest {
    name: string;
    email: string;
    password: string;
    surname: string;
}

export interface PostCompanyOwnerResponse {
    name: string;
    email: string;
    surname: string;
    photo: string;
    role: string[];
    id: number;
}

export interface GetUsersRequest {
    fullName: string | null;
    role: string | null;
}

export interface GetUsersResponse {
    users: GetUserResponse[];
    totalCount: number;
}

export interface GetUserResponse {
    id: number;
    name: string;
    surname: string;
    fullName: string;
    email: string;
    createdAt: string;
    roles: Role[];
    photoUrl: string;
}

export interface Role {
    id: number;
    kind: string;
}
