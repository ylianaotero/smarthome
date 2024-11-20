import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {ApiService} from '../../shared/api.service';
import {userRetrieveModel} from '../home-owners/create/signUpUserModel';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['../../../styles.css']
})
export class UserPanelComponent implements OnInit{

  userName: string;

  rol : string = "";

  user : userRetrieveModel | null = null;

  constructor(private router: Router, private api: ApiService) {
    const storedUser = localStorage.getItem('user');
    if(storedUser){
      this.user = JSON.parse(storedUser) as userRetrieveModel;
    }
    this.userName = this.user?.name || 'Usuario';
    for (const role of this.user?.roles || []) {
      if (role.kind === "Administrator") {
        this.rol = "administrator";
        break;
      } else {
        this.rol = "companyowner";
      }
    }
  }

  goToAdmin(): void {
    this.router.navigate(['/administrators']);
  }

  goToCompanyOwner(): void {
    this.router.navigate(['/company-owners']);
  }

  goToHomeOwner(): void {
    this.router.navigate(['/home-owners']);
  }

  ngOnInit(): void {

  }

}
