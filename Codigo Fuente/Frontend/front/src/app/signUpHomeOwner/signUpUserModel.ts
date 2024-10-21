export interface userRegistrationModel {
  name: string;
  email: string;
  surname: string;
  password: string;
  photo: string;
}

export class userRegistrationInstance implements userRegistrationModel {
  name: string;
  email: string;
  password: string;
  surname: string;
  photo: string;

  constructor(name: string, email: string, password: string, surname: string , photo : string ){
    this.name = name;
    this.email = email;
    this.password = password;
    this.surname = surname;
    this.photo = photo;
  }
}


export interface userRetrieveModel{
  name: string;
  email: string;
  surname: string;
  photo: string;
  roles: string[];
  id: number;
}
