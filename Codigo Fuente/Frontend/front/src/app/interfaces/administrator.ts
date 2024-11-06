//Crear un administrador
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

//Traer lista de usuarios
export interface GetUsersRequest {
    fullName: string | null;
    role: string | null;
}

export interface GetUsersResponse {
    users: GetUserResponse[];
}

export interface GetUserResponse {
    id: number;
    name: string;
    surname: string;
    fullName: string;
    createdAt: string;
    roles: Role[];
}

export interface Role {
    id: number;
    kind: string;
}