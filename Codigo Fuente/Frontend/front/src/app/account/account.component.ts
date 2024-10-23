import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../shared/api.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent {

  userName: string;

  constructor(private api: ApiService, private router: Router) {
    this.userName = this.api.currentSession?.user?.name || 'Usuario';
  }

  goCreateHome(): void {
    this.router.navigate(['/homes']);
  }

  goHomesOfHomeOwner(): void {
    this.router.navigate(['/homes-home-owner']);
  }

}
