import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../shared/api.service';
import { home } from './homeModels';

@Component({
  selector: 'app-homes-home-owner',
  templateUrl: './homesOfHomeOwner.component.html',
  styleUrls: ['./homesOfHomeOwner.component.css']
})
export class HomesOfHomeOwnerComponent implements OnInit {

  homes!: home[];
  isLoading: boolean = true;  // Para controlar el estado de carga

  constructor(private api: ApiService, private router: Router) { }

  ngOnInit(): void {
    this.getHomes();
  }


  getHomes(): void {
    this.api.getHomesOfHomeOwner().subscribe({
      next: (res: any) => {
        this.homes = res.homes;
      }
    });
  }


}
