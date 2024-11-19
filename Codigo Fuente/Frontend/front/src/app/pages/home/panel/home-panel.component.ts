import { Component } from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-panel.component.html',
  styleUrls: ['./home-panel.component.css', '../../../../styles.css']
})

export class HomePanelComponent {
  title = 'app';

  constructor(private router: Router) {}

  goToLogin(): void {
    this.router.navigate(['/login']);
  }

  goToRegister(): void {
    this.router.navigate(['/home-owners/create']);
  }
}
