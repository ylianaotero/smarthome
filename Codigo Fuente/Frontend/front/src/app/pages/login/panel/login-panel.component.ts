import { Component } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Router } from '@angular/router';
import {sessionModel, sessionRequest} from './sessionModel';
import {userRetrieveModel} from '../../home-owners/create/signUpUserModel';

@Component({
  selector: 'app-log-in',
  templateUrl: './login-panel.component.html',
  styleUrls: ['../../../../styles.css']
})
export class LoginPanelComponent {
  feedback: string = "";
  userEmail: string = '';
  userPassword: string = '';

  constructor(private api: ApiService, private router: Router) {}

  signInUser(email: string, password: string): void {
    this.feedback = "Loading...";

    if (!this.isValidEmail(email)) {
      this.feedback = "Invalid email";
      return;
    }

    this.api.postSession(new sessionRequest(email, password)).subscribe({
      next: res => {
        this.feedback = "success";

        localStorage.setItem('user', JSON.stringify(res.user));
        localStorage.setItem('token', res.token);

        this.redirection();

      },
      error: err => {
        this.feedback = "Invalid credentials";
        if (err.status === 0) {
          this.feedback = "Could not connect to the server, please try again later.";
        }
      }
    });
  }

  redirection(){
    const storedUser = localStorage.getItem('user');
    let res: userRetrieveModel | null = null;
    if(storedUser){
      res = JSON.parse(storedUser) as userRetrieveModel;
    }
    console.log(res);
    if (res && Array.isArray(res.roles)) {
      const hasHomeOwnerRole = res.roles.some(role => role.kind === 'HomeOwner');
      const hasCompanyOwnerRole = res.roles.some(role => role.kind=== 'CompanyOwner');
      const hasAdministratorRole = res.roles.some(role => role.kind === 'Administrator');

      console.log("res.roles.length")
      console.log(res.roles.length)

      if (hasHomeOwnerRole && res.roles.length == 1) {
        this.router.navigate(['/home-owners']);
      } else if (hasCompanyOwnerRole && res.roles.length == 1) {
        this.router.navigate(['/company-owners']);
      } else if (hasAdministratorRole && res.roles.length == 1) {
        this.router.navigate(['/administrators']);
      }else if(res.roles.length > 1){
        this.router.navigate(['/home/user-panel']);
      }
    } else {
      this.feedback = "Invalid credentials";
    }
  }

  goToRegister(): void {
    this.router.navigate(['/home-owners/create']);
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
    return emailRegex.test(email);
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }
}
