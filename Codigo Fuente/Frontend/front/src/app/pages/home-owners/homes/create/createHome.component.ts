import { Component } from '@angular/core';
import {Router} from '@angular/router';

import {ApiService} from '../../../../shared/api.service';
import {createHomeModel} from './createHomeModel';
import {userRetrieveModel} from '../../create/signUpUserModel';

@Component({
  selector: 'app-create-home',
  templateUrl: './createHome.component.html',
  styleUrls: ['../../../../../styles.css']
})

export class CreateHomeComponent {

  aliasValue: string = '';
  streetValue: string = '';
  doorNumberValue: number = 0;
  latitudeValue: number = 0;
  longitudeValue: number = 0;
  maximumMembersValue: number = 1;

  feedback: string = "";

  user : userRetrieveModel | null = null;

  constructor(private api: ApiService, private router: Router) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
  }

  goHome(): void {
    this.router.navigate(['/home-owners']);
  }

  registerHome(street: string, doorNumber: number, latitude: number, longitude: number, maximumMembers: number, alias: string): void {
    this.feedback = "Loading...";

    if (!street || doorNumber === null || latitude === null || longitude === null || maximumMembers === null || !alias) {
      this.feedback = "Por favor, completa todos los campos obligatorios.";
      return;
    }

    const userId = this.user?.id;

    if (!userId) {
      this.feedback = "Could not retrieve the user ID.";
      return;
    }

    this.api.postHome(new createHomeModel(userId, street, doorNumber, latitude, longitude, maximumMembers, alias))
      .subscribe({
        next: res => {
          this.feedback = "Home created successfully!";
          this.router.navigate(['/home-owners/homes']);
        },
        error: err => {
          this.handleError(err);
        }
      });
  }

  private handleError(err: any): void {
    switch (err.status) {
      case 400:
        this.feedback = "Invalid input. Please check your data.";
        break;
      case 409:
        this.feedback = "Home already exists.";
        break;
      case 412:
        this.feedback = "Precondition failed: " + err.error;
        break;
      case 0:
        this.feedback = "Could not connect to the server, please try again later.";
        break;
      default:
        this.feedback = "An unexpected error occurred. Please try again.";
    }
  }


}
