import { Component } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Router } from '@angular/router';
import { userRegistrationInstance } from './signUpUserModel';

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

  constructor(private api: ApiService, private router: Router) {}

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
          this.api.postSession({ email: email, password: password }).subscribe({
            next: (sessionInfo) => {
              this.api.currentSession = sessionInfo;
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

