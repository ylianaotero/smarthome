import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import {ApiService} from '../../shared/api.service';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['../../../styles.css']
})
export class UserPanelComponent implements OnInit{

  userName: string;

  rol : string = "";

  constructor(private router: Router, private api: ApiService) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
    console.log(this.api.currentSession?.user?.roles);
    for (const role of this.api.currentSession?.user?.roles || []) {
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
