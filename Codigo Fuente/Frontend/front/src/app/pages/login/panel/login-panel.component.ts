import { Component } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Router } from '@angular/router';
import {sessionModel, sessionRequest} from './sessionModel';

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
        this.api.currentSession = res;
        this.feedback = "success";
        localStorage.setItem('user', JSON.stringify(res));
        console.log(res.user.roles);
        this.redirection(res);

      },
      error: err => {
        this.feedback = "Invalid credentials";
        if (err.status === 0) {
          this.feedback = "Could not connect to the server, please try again later.";
        }
      }
    });
  }

  redirection(res: sessionModel){
    if (res.user && res.user.roles) {
      const hasHomeOwnerRole = res.user.roles.some(role => role.kind === 'HomeOwner');
      const hasCompanyOwnerRole = res.user.roles.some(role => role.kind === 'CompanyOwner');

      if (hasHomeOwnerRole) {
        this.router.navigate(['account']);
      } else if (hasCompanyOwnerRole) {
        this.router.navigate(['home/company-owner-panel']);
      } else {
        // Manejo si no tiene ninguno de los roles requeridos
      }
    } else {
      // Manejo si res.user o res.user.roles no existen
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
