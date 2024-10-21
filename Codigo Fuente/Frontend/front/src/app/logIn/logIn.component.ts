import { Component } from '@angular/core';
import { ApiService } from '../shared/api.service';
import { Router } from '@angular/router';
import { sessionRequest } from './sessionModel';

@Component({
  selector: 'app-log-in',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css']
})
export class LogInComponent {
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
        this.router.navigate(['account']);
      },
      error: err => {
        this.feedback = "Invalid credentials";
        if (err.status === 0) {
          this.feedback = "Could not connect to the server, please try again later.";
        }
      }
    });
  }

  goToRegister(): void {
    this.router.navigate(['/home-owners']);
  }

  isValidEmail(email: string): boolean {
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
    return emailRegex.test(email);
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }
}
