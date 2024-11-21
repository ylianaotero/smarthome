import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {userRegistrationInstance, userRetrieveModel} from './signUpUserModel';
import {ApiSessionService} from '../../../shared/session.service';
import {ApiUserService} from '../../../shared/user.service';

@Component({
  selector: 'app-sign-up-home-owner',
  templateUrl: './create-home-owner.component.html',
  styleUrls: ['../../../../styles.css']
})
export class CreateHomeOwnerComponent {

  feedback: string = "";
  userName: string = '';
  userSurname: string = '';
  userEmail: string = '';
  userPassword: string = '';
  userPhoto: string = '';

  user : userRetrieveModel | null = null;

  constructor(private api: ApiUserService, private router: Router, private sessionApi : ApiSessionService) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
  isValidEmail(email: string): boolean {
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
    let ret = emailRegex.test(email);
    return ret;
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }


  registerUser(name: string, email: string, password: string, surname: string, photo: string): void {
    this.feedback = "Loading...";

    if (!this.isValidEmail(email)) {
      this.feedback = "Invalid email";
      return;
    }


    this.api.postHomeOwner(new userRegistrationInstance(name, email, password, surname, photo))
      .subscribe({
        next: res => {
          this.feedback = "Registration successful!";
          this.sessionApi.postSession({ email: email, password: password }).subscribe({
            next: (sessionInfo) => {
              this.router.navigate(['/home-owners']);
            }
          })
        },
        error: err => {
          switch (err.status) {
            case 400:
              this.feedback =  "Invalid input. Please check your data.";
              break;
            case 409:
              this.feedback =  "User already exists.";
              break;
            case 0:
              this.feedback = "Could not connect to the server, please try again later.";
              break;
            default:
              this.feedback = "An unexpected error occurred. Please try again.";
          }
        }
      });

  }

}

