export interface userRegistrationModel {
  name: string;
  email: string;
  address: string;
  password: string;
}

export class userRegistrationInstance implements userRegistrationModel {
  name: string;
  email: string;
  address: string;
  password: string;
  constructor() {
    this.name = "";
    this.email = "";
    this.address = "";
    this.password = "";
  }
}

export interface userModel{
  id: string;
  name: string;
  email: string;
  address: string;
  password: string;
}

export interface userRetrieveModel{
  guid: string;
  name: string;
  email: string;
  address: string;
  roles: string[];
}
