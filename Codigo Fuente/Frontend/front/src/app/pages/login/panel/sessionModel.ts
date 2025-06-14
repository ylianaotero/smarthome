import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';

export interface sessionModel{
  token: string;
  user: userRetrieveModel;
}

export class sessionRequest{
  email: string;
  password: string;
  constructor(email: string, password: string){
    this.email = email;
    this.password = password;
  }
}
